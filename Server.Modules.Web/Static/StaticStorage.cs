using System.Buffers.Binary;
using System.IO;
using System.Threading.Tasks;
using Server.Common;
using Server.Common.Utilities;
using Server.ReNote;

namespace Server.Web.Static
{
    internal class StaticStorage
    {
        /// <summary>
        /// The chunk size of a file.
        /// </summary>
        private const byte CHUNK_SIZE  = 16;
        /// <summary>
        /// The size of a <see cref="long"/>.
        /// </summary>
        private const byte LONG_SIZE   = 8;
        /// <summary>
        /// The offset size.
        /// </summary>
        private const byte OFFSET_SIZE = 1;

        /// <summary>
        /// Returns a resource from the specified URI.
        /// </summary>
        /// <param name="uri">The resource URI.</param>
        /// <returns><see cref="byte"/>[]</returns>
        public static async Task<byte[]> GetResourceAsync(string uri, long requesterId)
        {
            string resourcePath = GetResourcePath(uri);
            if (!File.Exists(resourcePath))
                return WebResources.NotFoundError;

            byte[] resource = await File.ReadAllBytesAsync(resourcePath);
            if (!HasAttribute(resource))
                return resource;

            long ownerId = GetIDAttribute(resource);
            byte[] newResource = GetOriginalResource(resource);

            if (ownerId == Constants.PUBLIC_AUTH_ID || ownerId == Constants.SHARED_AUTH_ID && requesterId != Constants.PUBLIC_AUTH_ID || ownerId == requesterId)
                return newResource;

            return WebResources.NotFoundError;
        }

        /// <summary>
        /// Returns whether the resource exists or not.
        /// </summary>
        /// <param name="uri">The resource URI.</param>
        /// <returns><see cref="bool"/></returns>
        public static bool ResourceExists(string uri)
        {
            return File.Exists(GetResourcePath(uri));
        }

        /// <summary>
        /// Returns an absolute path representing the path of a file from an URI.
        /// </summary>
        /// <param name="uri">The resource URI.</param>
        /// <returns><see cref="string"/></returns>
        private static string GetResourcePath(string uri)
        {
            if (GetPathExtension(uri) == "None")
                return WebResources.Index;
            else if (GetPathExtension(uri) == "DotHtml")
                return PathUtil.NormalizeToOS($"{WebResources.WebRoot}/{uri}/index.html");
            else if (GetPathExtension(uri) == "VueHtml")
                return WebResources.Index;
            else
                return PathUtil.NormalizeToOS($"{WebResources.WebRoot}/{uri}");
        }

        /// <summary>
        /// Returns the type of file represented in the URI.
        /// </summary>
        /// <param name="uri">The resource URI.</param>
        /// <returns><see cref="string"/></returns>
        private static string GetPathExtension(string uri)
        {
            if (string.IsNullOrWhiteSpace(uri) || uri == "/")
                return "None";
            else if (Configuration.WebConfig.WebNoDotHtml && uri.LastIndexOf('.') == -1)
                return "DotHtml";
            else if (Configuration.WebConfig.WebVueSupport && uri.LastIndexOf('.') == -1)
                return "VueHtml";
            else
                return "Other";
        }

        /// <summary>
        /// Gets the original resource data. (without attributes)
        /// </summary>
        /// <param name="resource">The resource to be proceeded.</param>
        /// <returns></returns>
        private static byte[] GetOriginalResource(byte[] resource)
        {
            // We remove the total number of bytes used by the attributes.
            int offset = CHUNK_SIZE - resource[resource.Length - CHUNK_SIZE];
            int totalSize = CHUNK_SIZE + offset;

            byte[] newResource = new byte[resource.Length - totalSize];
            for (int i = 0; i < newResource.Length; i++)
                newResource[i] = resource[i];

            return  newResource;
        }

        /// <summary>
        /// Returns whether the file has attributes.
        /// </summary>
        /// <param name="resource">The resource to be proceeded.</param>
        /// <returns><see cref="bool"/></returns>
        private static bool HasAttribute(byte[] resource)
        {
            // We check if the final attribute's bytes are empty. (the ones that are not used)
            byte finalBytes = CHUNK_SIZE - OFFSET_SIZE - LONG_SIZE;
            for (int i = resource.Length - finalBytes; i < resource.Length; i++)
            {
                if (resource[i] != 0)
                    return false;
            }

            return true;
        }

        /// <summary>
        /// Returns the actual owner of the file.
        /// </summary>
        /// <param name="resource">The resource to be proceeded.</param>
        /// <returns><see cref="long"/></returns>
        private static long GetIDAttribute(byte[] resource)
        {
            // The User ID starts at offset 1.
            byte[] arrayOwnerId = new byte[LONG_SIZE];
            byte offset = CHUNK_SIZE - OFFSET_SIZE;
            for (int i = 0; i < arrayOwnerId.Length; i++)
                arrayOwnerId[i] = resource[resource.Length - offset + i];

            return BinaryPrimitives.ReadInt64BigEndian(arrayOwnerId);
        }
    }
}
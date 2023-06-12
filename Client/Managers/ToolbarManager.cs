namespace Client.Managers;

using Client.Api.Responses;
using Client.Pages;
using System;
using System.Collections.Generic;
using System.Linq;

internal class ToolbarManager
{
    private static readonly Dictionary<string, Toolbar> s_Toolbars = new Dictionary<string, Toolbar>();

    public static void Initialize(ToolbarInfo[] toolbarsInfo)
    {
        if (toolbarsInfo == null)
            return;

        for (int i = 0; i < toolbarsInfo.Length; i++)
        {
            ToolbarInfo toolbarInfo = toolbarsInfo[i];
            Toolbar toolbar = new Toolbar(toolbarInfo.Id, toolbarInfo.Name)
            {
                DefaultPage = Type.GetType($"Client.Pages.{toolbarInfo.DefaultPage}")
            };

            for (int j = 0; j < toolbarInfo.Buttons.Count; j++)
            {
                KeyValuePair<string, string> button = toolbarInfo.Buttons.ElementAt(j);
                toolbar.Buttons.Add(button.Key, Type.GetType($"Client.Pages.{button.Value}"));
            }

            AddToolbar(toolbar);
        }
    }

    public static void AddToolbar(Toolbar toolbar)
    {
        if (s_Toolbars.ContainsKey(toolbar.Id))
            return;

        s_Toolbars.Add(toolbar.Id, toolbar);
    }

    public static Toolbar GetToolbar(string toolbarId)
    {
        if (string.IsNullOrWhiteSpace(toolbarId))
            return null;

        if (!s_Toolbars.ContainsKey(toolbarId))
            return null;

        return s_Toolbars[toolbarId];
    }
}
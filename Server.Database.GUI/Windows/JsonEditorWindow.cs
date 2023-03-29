using System;
using System.Drawing;
using System.Linq;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace Server.Database.Windows
{
    public partial class JsonEditorWindow : Form
    {
        private JsonException JsonParseError;

        private Color m_BraceColor = Color.Blue;
        private Color m_StringColor = Color.Green;
        private Color m_NumberColor = Color.DarkOrange;
        private Color m_KeywordColor = Color.Red;
        private Color m_DefaultColor = Color.Black;

        private Regex m_BraceRegex = new Regex(@"[\{\}]");
        private Regex m_BracketRegex = new Regex(@"[\[\]]");
        private Regex m_StringRegex = new Regex("\"(?:\\\\.|[^\"])*\"");
        private Regex m_NumberRegex = new Regex(@"-?\d+(?:\.\d+)?");
        private Regex m_KeywordRegex = new Regex(@"\b(?:true|false|null)\b");

        public string Data { get; private set; }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="data">The data that's going to be displayed by default.</param>
        public JsonEditorWindow(string data)
        {
            InitializeComponent();

            jsonEditor.BorderStyle = BorderStyle.None;
            jsonEditor.Font = new Font("Consolas", 12);
            jsonEditor.BackColor = Color.White;
            jsonEditor.Multiline = true;
            jsonEditor.AcceptsTab = true;
            jsonEditor.WordWrap = false;

            Beautify(data);
            HandleSyntaxHighlighting();

            jsonEditor.KeyDown += OnEditorKeyDown;
            jsonEditor.TextChanged += OnEditorTextChanged;
        }

        /// <summary>
        /// Occurs when a key is down while focusing the <see cref="jsonEditor"/>.
        /// Handles specific keys.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The event arguments.</param>
        private void OnEditorKeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Tab)
            {
                jsonEditor.SelectedText = "\u0020".PadRight(4);
                e.SuppressKeyPress = true;
                e.Handled = true;
            }
            else if (e.KeyCode == Keys.Enter)
            {
                e.Handled = HandleFormatting();
            }
        }

        /// <summary>
        /// Occurs when a key is pressed while focusing the <see cref="jsonEditor"/>.
        /// Handles specific keys.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The event arguments.</param>
        private void OnEditorKeyPress(object sender, KeyPressEventArgs e)
        {

            if (e.KeyChar == '{')
            {
                e.Handled = true;
                int selectionStart = jsonEditor.SelectionStart;
                jsonEditor.Text = jsonEditor.Text.Insert(selectionStart, "{}");
                jsonEditor.SelectionStart = selectionStart + 1;
            }
            else if (e.KeyChar == '[')
            {
                e.Handled = true;
                int selectionStart = jsonEditor.SelectionStart;
                jsonEditor.Text = jsonEditor.Text.Insert(selectionStart, "[]");
                jsonEditor.SelectionStart = selectionStart + 1;
            }
            else if (e.KeyChar == '"')
            {
                e.Handled = true;
                int selectionStart = jsonEditor.SelectionStart;
                jsonEditor.Text = jsonEditor.Text.Insert(selectionStart, "\"\"");
                jsonEditor.SelectionStart = selectionStart + 1;
            }
        }

        /// <summary>
        /// Occurs when the text inside the <see cref="jsonEditor"/> is modified.
        /// Applies syntax highlighting.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The event arguments.</param>
        private void OnEditorTextChanged(object sender, EventArgs e)
        {
            if (JsonParseError != null)
            {
                detailsLabel.Visible = false;
                detailsButton.Visible = false;
                JsonParseError = null;
            }

            HandleSyntaxHighlighting();
        }

        /// <summary>
        /// Occurs when the confirm button is clicked.
        /// Minifies the JSON data and closes the dialog.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The event arguments.</param>
        private void OnConfirmButtonClicked(object sender, EventArgs e)
        {
            Minify();

            if (JsonParseError != null)
                detailsButton.PerformClick();
            else
                this.Close();
        }

        /// <summary>
        /// Occurs when the cancel button is clicked.
        /// Closes the dialog.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The event arguments.</param>
        private void OnCancelButtonClicked(object sender, EventArgs e)
        {
            this.Close();
        }

        /// <summary>
        /// Applies syntax highlighting.
        /// </summary>
        private void HandleSyntaxHighlighting()
        {
            int originalIndex = jsonEditor.SelectionStart;
            int originalLength = jsonEditor.SelectionLength;

            editorLabel.Focus();

            jsonEditor.SelectionStart = 0;
            jsonEditor.SelectionLength = jsonEditor.Text.Length;
            jsonEditor.SelectionColor = m_DefaultColor;

            MatchCollection matches = m_BraceRegex.Matches(jsonEditor.Text);
            for (int i = 0; i < matches.Count; i++)
            {
                jsonEditor.SelectionStart = matches[i].Index;
                jsonEditor.SelectionLength = 1;
                jsonEditor.SelectionColor = m_BraceColor;
            }

            matches = m_BracketRegex.Matches(jsonEditor.Text);
            for (int i = 0; i < matches.Count; i++)
            {
                jsonEditor.SelectionStart = matches[i].Index;
                jsonEditor.SelectionLength = 1;
                jsonEditor.SelectionColor = m_BraceColor;
            }

            matches = m_KeywordRegex.Matches(jsonEditor.Text);
            for (int i = 0; i < matches.Count; i++)
            {
                jsonEditor.SelectionStart = matches[i].Index;
                jsonEditor.SelectionLength = matches[i].Length;
                jsonEditor.SelectionColor = m_KeywordColor;
            }

            matches = m_NumberRegex.Matches(jsonEditor.Text);
            for (int i = 0; i < matches.Count; i++)
            {
                jsonEditor.SelectionStart = matches[i].Index;
                jsonEditor.SelectionLength = matches[i].Length;
                jsonEditor.SelectionColor = m_NumberColor;
            }

            matches = m_StringRegex.Matches(jsonEditor.Text);
            for (int i = 0; i < matches.Count; i++)
            {
                jsonEditor.SelectionStart = matches[i].Index;
                jsonEditor.SelectionLength = matches[i].Length;
                jsonEditor.SelectionColor = m_StringColor;
            }

            jsonEditor.SelectionStart = originalIndex;
            jsonEditor.SelectionLength = originalLength;
            jsonEditor.SelectionColor = m_DefaultColor;

            jsonEditor.Focus();
        }

        /// <summary>
        /// Occurs when the details button is clicked.
        /// Shows details about a JSON syntax error.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The event arguments.</param>
        private void OnDetailsButtonClicked(object sender, EventArgs e)
        {
            if (JsonParseError == null)
                return;

            MessageBox.Show(JsonParseError.ToString(),
                            "JSON Parse Error Stacktrace",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Error);
        }

        /// <summary>
        /// Applies formatting.
        /// Returns whether the TextChanged event should be handled.
        /// </summary>
        /// <returns><see cref="bool"/></returns>
        private bool HandleFormatting()
        {
            int cursorPosition = jsonEditor.SelectionStart;
            if (cursorPosition < 1 || cursorPosition > jsonEditor.Text.Length)
                return false;

            char prevChar = jsonEditor.Text[cursorPosition - 1];
            char nextChar = jsonEditor.Text[cursorPosition];

            int lineNumber = jsonEditor.GetLineFromCharIndex(cursorPosition);
            string lineText = jsonEditor.Lines[lineNumber];

            int identationCount = lineText.TakeWhile(c => c == '\u0020').Count();
            string identations = string.Empty;
            string newIdentations = "\u0020".PadRight(identationCount + 4);
            for (int i = 0; i < identationCount; i++)
                identations += "\u0020";

            switch ($"{prevChar}{nextChar}")
            {
                case "{}":
                    break;
                case "[]":
                    break;
                default:
                    jsonEditor.SelectedText = $"{Environment.NewLine}{identations}";
                    return true;
            }

            jsonEditor.SelectedText = $"{Environment.NewLine}{newIdentations}{Environment.NewLine}{identations}";
            jsonEditor.SelectionStart = cursorPosition + identationCount + 5;
            return true;
        }

        /// <summary>
        /// Minifies the data.
        /// </summary>
        private void Minify()
        {
            try
            {
                string jsonText = jsonEditor.Text;
                Data = JsonSerializer.Serialize(JsonSerializer.Deserialize<JsonDocument>(jsonText));
            }
            catch (JsonException ex)
            {
                detailsLabel.Visible = true;
                detailsButton.Visible = true;
                JsonParseError = ex;
            }
        }

        /// <summary>
        /// Beautifies the data.
        /// </summary>
        /// <param name="data">The data to be beautified.</param>
        private void Beautify(string data)
        {
            try
            {
                using JsonDocument document = JsonDocument.Parse(data);
                jsonEditor.Text = JsonSerializer.Serialize(document, new JsonSerializerOptions()
                {
                    WriteIndented = true
                });
            }
            catch (JsonException ex)
            {
                detailsLabel.Visible = true;
                detailsButton.Visible = true;
                JsonParseError = ex;
            }
        }
    }
}
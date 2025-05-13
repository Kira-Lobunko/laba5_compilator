using System.Text;
using System.Windows.Forms;

namespace CompilerApp
{
    public partial class MainMenuForm : Form // �������� ����� ����������
    {
        private string? currentFilePath = null; // ������� ���� �����
        private bool isTextChanged = false; // ������������ ��������� ������

        public MainMenuForm()
        {
            InitializeComponent();
            fontSizeComboBox.SelectedItem = "9"; // ��������� ������ ������ (������� ��������������, ������ �����������)
            AttachInputAreaEvents(); // �������� ������� � ������� �������������� (inputArea)
        }

        // ����� �������� ������� � ������� �������������� (inputArea)
        private void AttachInputAreaEvents()
        {
            // �������� ������������ ������� �������������� � ����������
            // ����� � ������� �������������� (inputArea)
            inputArea.AllowDrop = true;
            inputArea.DragEnter += InputArea_DragEnter;
            inputArea.DragDrop += InputArea_DragDrop;

            // ���������� ��������� ����� ��� ������������ ���������, ��������� ������, ��������� ������� �������, 
            inputArea.VScroll += (s, e) => UpdateLineNumbers(inputArea, lineNumbersBox);
            inputArea.TextChanged += (s, e) => UpdateLineNumbers(inputArea, lineNumbersBox);
            inputArea.Resize += (s, e) => UpdateLineNumbers(inputArea, lineNumbersBox);
        }

        // ����������� �������
        private void ItemCreateFile_Click(object sender, EventArgs e) // ���������� ������� �������� �����
        {
            FileActions.CreateFile(this);
        }

        private void ItemOpenFile_Click(object sender, EventArgs e) // ���������� ������� �������� �����
        {
            FileActions.OpenFile(this);
        }

        private void ItemSaveFile_Click(object sender, EventArgs e) // ���������� ������� ���������� �����
        {
            FileActions.SaveFile(this);
        }

        private void ItemSaveFileAs_Click(object sender, EventArgs e) // ���������� ������� ���������� ����� ���
        {
            FileActions.SaveFileAs(this);
        }

        private void ItemExit_Click(object sender, EventArgs e) // ���������� ������� ������ �� ����������
        {
            FileActions.ExitApplication(this);
        }

        private void InputArea_TextChanged(object sender, EventArgs e) // ���������� ������� ��������� ������
        {
            SetTextChanged(true);
        }

        private void ItemUndo_Click(object sender, EventArgs e) // ���������� ������� ������ ����������� ��������� � ������
        {
            EditActions.Undo(this);
        }

        private void ItemRedo_Click(object sender, EventArgs e) // ���������� ������� ���������� ����������� ��������� � ������
        {
            EditActions.Redo(this);
        }

        private void ItemCut_Click(object sender, EventArgs e) // ���������� ������� - �������� ��������� ��������
        {
            EditActions.Cut(this);
        }

        private void ItemCopy_Click(object sender, EventArgs e) // ���������� ������� - ���������� ��������� ��������
        {
            EditActions.Copy(this);
        }

        private void ItemPaste_Click(object sender, EventArgs e) // ���������� ������� - �������� ��������� ��������
        {
            EditActions.Paste(this);
        }

        private void ItemDelete_Click(object sender, EventArgs e) // ���������� ������� - ������� ��������� ��������
        {
            EditActions.Delete(this);
        }

        private void ItemSelectAll_Click(object sender, EventArgs e) // ���������� ������� - �������� ���� �����
        {
            EditActions.SelectAll(this);
        }

        private void ItemSettingTask_Click(object sender, EventArgs e) // ���������� ������� ������ "���������� ������" (�� ���)
        {
            TextActions.ShowSettingTask();
        }

        private void ItemGrammar_Click(object sender, EventArgs e) // ���������� ������� ������ "����������" (�� ���)
        {
            TextActions.ShowGrammar();
        }

        private void ItemGrammarClassification_Click(object sender, EventArgs e) // ���������� ������� ������ "������������� ����������" (�� ���)
        {
            TextActions.ShowGrammarClassification();
        }

        private void ItemMethodOfAnalysis_Click(object sender, EventArgs e) // ���������� ������� ������ "����� �������" (�� ���)
        {
            TextActions.ShowMethodOfAnalysis();
        }

        // ���������� ������� ������ "����������� � ������������� ������" (�� ���)
        private void ItemDiagnosisAndNeutralizationOfErrors_Click(object sender, EventArgs e)
        {
            TextActions.ShowDiagnosisAndNeutralizationOfErrors();
        }

        private void ItemTestExamples_Click(object sender, EventArgs e) // ���������� ������� ������ "�������� �������" (�� ���)
        {
            TextActions.ShowTestExamples();
        }

        private void ItemListOfLiterature_Click(object sender, EventArgs e) // ���������� ������� ������ "������ ����������" (�� ���)
        {
            TextActions.ShowListOfLiterature();
        }

        private void ItemSourceCodeOfProgram_Click(object sender, EventArgs e) // ���������� ������� ������ "�������� ��� ���������" (�� ���)
        {
            TextActions.ShowSourceCodeOfProgram();
        }

        private void ItemLaunch_Click(object sender, EventArgs e) // ���������� ������� - ������ �����������
        {
            // ������ ����������� ��� "���������� ��������� ������� �� ����� C/C++"
            //LaunchActions.Launch(this);

            // ������ ����������� ��� "�������� ��������� ������ (�����)"
            LaunchActions.LaunchPolish(this);
        }

        private void ItemCallHelp_Click(object sender, EventArgs e) // ���������� ������� ������ �������
        {
            HelpActions.ShowHelp();
        }

        private void ItemAboutProgram_Click(object sender, EventArgs e) // ���������� ������� ������ "� ���������"
        {
            HelpActions.ShowAboutProgram();
        }

        // ���������� ������� �������� ����� (����� �������)
        private void MainMenuForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (IsTextChanged())
            {
                var result = MessageBox.Show(
                    "��������� ��������� ����� �������?",
                    "������������� ���������",
                    MessageBoxButtons.YesNoCancel,
                    MessageBoxIcon.Warning
                );

                if (result == DialogResult.Yes)
                {
                    if (!FileActions.SaveFile(this))
                    {
                        e.Cancel = true; // ���� ���������� �� ������, �������� ��������
                    }
                }
                else if (result == DialogResult.Cancel)
                {
                    e.Cancel = true; // �������� ��������, ���� ������������ ����� �������
                }
            }
        }

        // ������ � RichTextBox � DataGridView

        // ���������� ������ inputArea (������� �����)
        public RichTextBox GetInputArea() => inputArea;

        // ���������� ������ outputTable (������� ������)
        public DataGridView GetOutputTable() => outputTable;

        // ���������� ������ errorsTable (������� ������)
        public DataGridView GetErrorsTable() => errorsTable;

        // ���������� ������ outputArea (������� ������)
        public RichTextBox GetOutputArea() => outputArea;

        // ������ � ���� �����
        public void SetCurrentFilePath(string path) // ������������� ������� ���� �����
        {
            currentFilePath = path;
        }

        public string? GetCurrentFilePath() // ���������� ������� ���� �����
        {
            return currentFilePath;
        }

        // ������ � ����������� ������
        public void SetTextChanged(bool changed) // ������������� ���� ��������� ������ (true - ����� ������, false - ����� �� ������)
        {
            isTextChanged = changed;
        }

        public bool IsTextChanged() // ���������� ���� ��������� ������
        {
            return isTextChanged;
        }

        // �������������� �����������

        // ���������� ������� ������ ������� ������ �� ComboBox
        private void FontSizeComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (int.TryParse(fontSizeComboBox.SelectedItem.ToString(), out int fontSize))
            {
                ApplyFontSizeToAll(fontSize); // ��������� ������ �� ���� �����������
                UpdateLineNumbers(inputArea, lineNumbersBox); // ��������� ��������� ����� (������ ������ ������ �� ���������� �����)
            }
        }

        // ����� ��������� ������ ��� ���� ���������� �����������
        private void ApplyFontSizeToAll(int fontSize)
        {
            Font newFont = new Font(inputArea.Font.FontFamily, fontSize);

            inputArea.Font = newFont;
            lineNumbersBox.Font = newFont;

            outputTable.DefaultCellStyle.Font = newFont;
            outputTable.ColumnHeadersDefaultCellStyle.Font = newFont;

            errorsTable.DefaultCellStyle.Font = newFont;
            errorsTable.ColumnHeadersDefaultCellStyle.Font = newFont;

            outputArea.Font = newFont;
        }

        // ���������� ������� ������� ������ (������� ������� ��� ������� ������)
        private void MainMenuForm_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.N) // Ctrl + N � ������� ����� ����
            {
                FileActions.CreateFile(this);
                e.SuppressKeyPress = true;       // ������ ������� ������������
            }
            else if (e.Control && e.KeyCode == Keys.O) // Ctrl + O � ������� ����
            {
                FileActions.OpenFile(this);
                e.SuppressKeyPress = true;
            }
            else if (e.Control && e.KeyCode == Keys.S && !e.Shift) // Ctrl + S � ���������
            {
                FileActions.SaveFile(this);
                e.SuppressKeyPress = true;
            }
            else if (e.Control && e.KeyCode == Keys.S && e.Shift) // Ctrl + Shift + S � ��������� ���
            {
                FileActions.SaveFileAs(this);
                e.SuppressKeyPress = true;
            }
            else if (e.Control && e.KeyCode == Keys.Q) // Ctrl + Q � �����
            {
                FileActions.ExitApplication(this);
                e.SuppressKeyPress = true;
            }
            else if (e.Control && e.KeyCode == Keys.Z) // Ctrl + Z � ������
            {
                EditActions.Undo(this);
                e.SuppressKeyPress = true;
            }
            else if (e.Control && e.KeyCode == Keys.Y) // Ctrl + Y � ������
            {
                EditActions.Redo(this);
                e.SuppressKeyPress = true;
            }
            else if (e.Control && e.KeyCode == Keys.X) // Ctrl + X � ��������
            {
                EditActions.Cut(this);
                e.SuppressKeyPress = true;
            }
            else if (e.Control && e.KeyCode == Keys.C) // Ctrl + C � ����������
            {
                EditActions.Copy(this);
                e.SuppressKeyPress = true;
            }
            else if (e.Control && e.KeyCode == Keys.V) // Ctrl + V � ��������
            {
                EditActions.Paste(this);
                e.SuppressKeyPress = true;
            }
            else if (e.Control && e.KeyCode == Keys.A) // Ctrl + A � �������� ��
            {
                EditActions.SelectAll(this);
                e.SuppressKeyPress = true;
            }
            else if (e.KeyCode == Keys.F1) // F1 � �������
            {
                HelpActions.ShowHelp();
                e.SuppressKeyPress = true;
            }
        }

        // ���������� ������� �������������� ����� � ������� ��������������
        private void InputArea_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
                e.Effect = DragDropEffects.Copy;
            else
                e.Effect = DragDropEffects.None;
        }

        // ���������� ������� ���������� ����� � ������� ��������������
        private void InputArea_DragDrop(object sender, DragEventArgs e)
        {
            string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);

            if (files.Length > 0 && Path.GetExtension(files[0]).Equals(".txt", StringComparison.OrdinalIgnoreCase))
            {
                string filePath = files[0];
                inputArea.Text = File.ReadAllText(filePath);
                SetCurrentFilePath(filePath);
                SetTextChanged(false);

                UpdateStatus($"������ ����: {Path.GetFileName(filePath)}"); // ���������� ������ ���������
            }
            else
            {
                MessageBox.Show("����� ������� ������ ��������� ����� (*.txt).", "������", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        // �������������� ������

        // ����� ���������� ������ ���������
        public void UpdateStatus(string message)
        {
            statusLabel.Text = message;
        }

        // ����� ���������� ��������� ����� � ���� �������������� ������ (inputArea)
        private void UpdateLineNumbers(RichTextBox inputArea, RichTextBox lineNumbersBox)
        {
            if (inputArea == null || lineNumbersBox == null) return;

            // ��������� ������ ������ ������������ ������
            int firstIndex = inputArea.GetCharIndexFromPosition(new Point(0, 0));
            int firstLine = inputArea.GetLineFromCharIndex(firstIndex);

            // ��������� ������ ��������� ������������ ������
            int lastIndex = inputArea.GetCharIndexFromPosition(new Point(0, inputArea.Height));
            int lastLine = inputArea.GetLineFromCharIndex(lastIndex);

            // ������� � ���������� ������� �����
            lineNumbersBox.Clear();
            for (int i = firstLine; i <= lastLine; i++)
            {
                lineNumbersBox.AppendText((i + 1) + Environment.NewLine);
            }
        }

        // ������ ������������ �������

        // ����� ������������ �� ������� tabPageTokens
        public void SelectTokensTab()
        {
            tabControlOutput.SelectedTab = tabPageTokens;
        }

        // ����� ������������ �� ������� tabPageErrors
        public void SelectErrorsTab()
        {
            tabControlOutput.SelectedTab = tabPageErrors;
        }

        // ����� ������������ �� ������� tabPageResults
        public void SelectResultsTab()
        {
            tabControlOutput.SelectedTab = tabPageResults;
        }

        // ������ ��� ��������� ������ � ���������

        // ����� ��������� ������ � ���������
        internal void HighlightErrors(List<ParseError> errors)
        {
            // ������� ������� ��� ���������� ���������
            inputArea.SelectAll();
            inputArea.SelectionBackColor = inputArea.BackColor;

            // ������������ ������ ������ �� ������
            foreach (var error in errors)
            {
                // �������� ������� ������� �� ������ (������ X, ������� A-B)
                if (TryExtractPosition(error.Position, out int line, out int start, out int end))
                {
                    try
                    {
                        // �������� ������ ������� ������� ������ ������
                        int charIndex = inputArea.GetFirstCharIndexFromLine(line - 1);

                        // ��������� ���������� ������� ������ ������ � ������
                        int selectionStart = charIndex + start - 1;

                        // ����� ��������� = (����� - ������) + 1
                        int length = end - start + 1;

                        // �������� ��������� ������� � ����������� ���
                        inputArea.Select(selectionStart, length);
                        inputArea.SelectionBackColor = Color.Pink;
                    }
                    catch
                    {
                        // ���� ��������� ������ (��������, ������ �� ��������� ������) � ����������
                    }
                }
            }

            // ������� ��������� � ���������� ������ � ����� ������
            inputArea.SelectionStart = inputArea.TextLength;
            inputArea.SelectionLength = 0;
            inputArea.SelectionBackColor = inputArea.BackColor;
        }

        // ����� ������� ���������� ������� �� ������
        private bool TryExtractPosition(string position, out int line, out int start, out int end)
        {
            line = start = end = 0;

            try
            {
                // ������ ������ �������: "������ 1, ������� 5-10"
                string[] parts = position.Split(',');

                // ��������� ����� ������: "������ 1" -> 1
                line = int.Parse(parts[0].Split(' ')[1]);

                // ��������� �������� ��������: "������� 5-10" -> 5 � 10
                string[] symbols = parts[1].Split(' ')[2].Split('-');
                start = int.Parse(symbols[0]);
                end = int.Parse(symbols[1]);
                return true;
            }
            catch
            {
                // ���� �� ������� ��������� ������� � ���������� false
                return false;
            }
        }
    }
}

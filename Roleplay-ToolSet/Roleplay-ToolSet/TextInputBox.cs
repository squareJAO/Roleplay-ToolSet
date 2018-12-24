using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;

namespace RoleplayToolSet
{
    public class TextInputBox : Form
    {
        private TextBox _textBox;
        public TextInputBox(string title, string prompt, string currentText="")
        {
            Label label = new Label();
            _textBox = new TextBox();
            Button buttonOk = new Button();
            Button buttonCancel = new Button();

            this.Text = title;
            label.Text = prompt;
            _textBox.Text = currentText;

            buttonOk.Text = "OK";
            buttonCancel.Text = "Cancel";
            buttonOk.DialogResult = DialogResult.OK;
            buttonCancel.DialogResult = DialogResult.Cancel;

            label.SetBounds(9, 20, 372, 13);
            _textBox.SetBounds(12, 36, 372, 20);
            buttonOk.SetBounds(228, 72, 75, 23);
            buttonCancel.SetBounds(309, 72, 75, 23);

            label.AutoSize = true;
            _textBox.Anchor = _textBox.Anchor | AnchorStyles.Right;
            buttonOk.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            buttonCancel.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;

            this.ClientSize = new Size(396, 107);
            this.Controls.AddRange(new Control[] { label, _textBox, buttonOk, buttonCancel });
            this.ClientSize = new Size(Math.Max(300, label.Right + 10), this.ClientSize.Height);
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.StartPosition = FormStartPosition.CenterScreen;
            this.MinimizeBox = false;
            this.MaximizeBox = false;
            this.AcceptButton = buttonOk;
            this.CancelButton = buttonCancel;
        }

        public string GetInputtedText()
        {
            return _textBox.Text;
        }
    }
}

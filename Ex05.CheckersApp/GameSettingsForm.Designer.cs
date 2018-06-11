using Ex05.CheckersLogic;

namespace Ex05.CheckersApp
{
    public partial class GameSettingsForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }

            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.labelBoardSize = new System.Windows.Forms.Label();
            this.radioBoardSize6 = new System.Windows.Forms.RadioButton();
            this.radioBoardSize8 = new System.Windows.Forms.RadioButton();
            this.radioBoardSize10 = new System.Windows.Forms.RadioButton();
            this.labelPkayers = new System.Windows.Forms.Label();
            this.labelPlayer1 = new System.Windows.Forms.Label();
            this.textBoxPlayer1 = new System.Windows.Forms.TextBox();
            this.checkBoxPlayer2 = new System.Windows.Forms.CheckBox();
            this.textBoxPlayer2 = new System.Windows.Forms.TextBox();
            this.settingsDoneButton = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // labelBoardSize
            // 
            this.labelBoardSize.AutoSize = true;
            this.labelBoardSize.Location = new System.Drawing.Point(25, 31);
            this.labelBoardSize.Name = "labelBoardSize";
            this.labelBoardSize.Size = new System.Drawing.Size(81, 17);
            this.labelBoardSize.TabIndex = 0;
            this.labelBoardSize.Text = "Board Size:";
            // 
            // radioBoardSize6
            // 
            this.radioBoardSize6.AutoSize = true;
            this.radioBoardSize6.Location = new System.Drawing.Point(52, 51);
            this.radioBoardSize6.Name = "radioBoardSize6";
            this.radioBoardSize6.Size = new System.Drawing.Size(62, 21);
            this.radioBoardSize6.TabIndex = 1;
            this.radioBoardSize6.TabStop = true;
            this.radioBoardSize6.Tag = Ex05.CheckersLogic.BoardSize.Small;
            this.radioBoardSize6.Text = "6 X 6";
            this.radioBoardSize6.UseVisualStyleBackColor = true;
            // 
            // radioBoardSize8
            // 
            this.radioBoardSize8.AutoSize = true;
            this.radioBoardSize8.Location = new System.Drawing.Point(120, 51);
            this.radioBoardSize8.Name = "radioBoardSize8";
            this.radioBoardSize8.Size = new System.Drawing.Size(62, 21);
            this.radioBoardSize8.TabIndex = 2;
            this.radioBoardSize8.TabStop = true;
            this.radioBoardSize8.Tag = Ex05.CheckersLogic.BoardSize.Medium;
            this.radioBoardSize8.Text = "8 X 8";
            this.radioBoardSize8.UseVisualStyleBackColor = true;
            // 
            // radioBoardSize10
            // 
            this.radioBoardSize10.AutoSize = true;
            this.radioBoardSize10.Location = new System.Drawing.Point(192, 51);
            this.radioBoardSize10.Name = "radioBoardSize10";
            this.radioBoardSize10.Size = new System.Drawing.Size(78, 21);
            this.radioBoardSize10.TabIndex = 3;
            this.radioBoardSize10.TabStop = true;
            this.radioBoardSize10.Tag = Ex05.CheckersLogic.BoardSize.Large;
            this.radioBoardSize10.Text = "10 X 10";
            this.radioBoardSize10.UseVisualStyleBackColor = true;
            // 
            // labelPkayers
            // 
            this.labelPkayers.AutoSize = true;
            this.labelPkayers.Location = new System.Drawing.Point(22, 87);
            this.labelPkayers.Name = "labelPkayers";
            this.labelPkayers.Size = new System.Drawing.Size(59, 17);
            this.labelPkayers.TabIndex = 4;
            this.labelPkayers.Text = "Players:";
            // 
            // labelPlayer1
            // 
            this.labelPlayer1.AutoSize = true;
            this.labelPlayer1.Location = new System.Drawing.Point(49, 121);
            this.labelPlayer1.Name = "labelPlayer1";
            this.labelPlayer1.Size = new System.Drawing.Size(60, 17);
            this.labelPlayer1.TabIndex = 5;
            this.labelPlayer1.Text = "Player1:";
            // 
            // textBoxPlayer1
            // 
            this.textBoxPlayer1.Location = new System.Drawing.Point(170, 116);
            this.textBoxPlayer1.Name = "textBoxPlayer1";
            this.textBoxPlayer1.Size = new System.Drawing.Size(100, 22);
            this.textBoxPlayer1.TabIndex = 6;
            // 
            // checkBoxPlayer2
            // 
            this.checkBoxPlayer2.AutoSize = true;
            this.checkBoxPlayer2.Location = new System.Drawing.Point(28, 153);
            this.checkBoxPlayer2.Name = "checkBoxPlayer2";
            this.checkBoxPlayer2.Size = new System.Drawing.Size(86, 21);
            this.checkBoxPlayer2.TabIndex = 7;
            this.checkBoxPlayer2.Text = "Player 2:";
            this.checkBoxPlayer2.UseVisualStyleBackColor = true;
            this.checkBoxPlayer2.CheckedChanged += new System.EventHandler(this.player2CheckBox_CheckedChanged);
            // 
            // textBoxPlayer2
            // 
            this.textBoxPlayer2.Enabled = false;
            this.textBoxPlayer2.Location = new System.Drawing.Point(170, 153);
            this.textBoxPlayer2.Name = "textBoxPlayer2";
            this.textBoxPlayer2.Size = new System.Drawing.Size(100, 22);
            this.textBoxPlayer2.TabIndex = 8;
            this.textBoxPlayer2.Text = "Computer";
            // 
            // settingsDoneButton
            // 
            this.settingsDoneButton.Location = new System.Drawing.Point(195, 219);
            this.settingsDoneButton.Name = "settingsDoneButton";
            this.settingsDoneButton.Size = new System.Drawing.Size(75, 23);
            this.settingsDoneButton.TabIndex = 9;
            this.settingsDoneButton.Text = "Done";
            this.settingsDoneButton.UseVisualStyleBackColor = true;
            this.settingsDoneButton.Click += new System.EventHandler(this.settingsDoneButton_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(0, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(46, 17);
            this.label1.TabIndex = 10;
            this.label1.Text = "label1";
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(0, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(100, 23);
            this.label2.TabIndex = 0;
            // 
            // GameSettingsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(288, 254);
            this.MinimizeBox = false;
            this.MaximizeBox = false;
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.settingsDoneButton);
            this.Controls.Add(this.textBoxPlayer2);
            this.Controls.Add(this.checkBoxPlayer2);
            this.Controls.Add(this.textBoxPlayer1);
            this.Controls.Add(this.labelPlayer1);
            this.Controls.Add(this.labelPkayers);
            this.Controls.Add(this.radioBoardSize10);
            this.Controls.Add(this.radioBoardSize8);
            this.Controls.Add(this.radioBoardSize6);
            this.Controls.Add(this.labelBoardSize);
            this.Name = "GameSettingsForm";
            this.Text = "Game Settings";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.RadioButton radioBoardSize6;
        private System.Windows.Forms.RadioButton radioBoardSize8;
        private System.Windows.Forms.RadioButton radioBoardSize10;
        private System.Windows.Forms.Label labelPkayers;
        private System.Windows.Forms.Label labelPlayer1;
        private System.Windows.Forms.TextBox textBoxPlayer1;
        private System.Windows.Forms.CheckBox checkBoxPlayer2;
        private System.Windows.Forms.TextBox textBoxPlayer2;
        private System.Windows.Forms.Label labelBoardSize;
        private System.Windows.Forms.Button settingsDoneButton;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
    }
}

namespace WindowsFormsApp1
{
    partial class Form1
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.addButton = new System.Windows.Forms.Button();
            this.editButton = new System.Windows.Forms.Button();
            this.deleteButton = new System.Windows.Forms.Button();
            this.listViewTasks = new System.Windows.Forms.ListView();
            this.comboBoxSort = new System.Windows.Forms.ComboBox();
            this.sortLabel = new System.Windows.Forms.Label();
            this.textBoxFilter = new System.Windows.Forms.TextBox();
            this.searchIcon = new System.Windows.Forms.PictureBox();
            this.checkBoxUncompleted = new System.Windows.Forms.CheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.searchIcon)).BeginInit();
            this.SuspendLayout();
            // 
            // addButton
            // 
            this.addButton.Location = new System.Drawing.Point(122, 401);
            this.addButton.Name = "addButton";
            this.addButton.Size = new System.Drawing.Size(75, 23);
            this.addButton.TabIndex = 1;
            this.addButton.Text = "Add";
            this.addButton.UseVisualStyleBackColor = true;
            this.addButton.Click += new System.EventHandler(this.AddButtonOnClick);
            // 
            // editButton
            // 
            this.editButton.Location = new System.Drawing.Point(467, 401);
            this.editButton.Name = "editButton";
            this.editButton.Size = new System.Drawing.Size(75, 23);
            this.editButton.TabIndex = 2;
            this.editButton.Text = "Edit";
            this.editButton.UseVisualStyleBackColor = true;
            this.editButton.Click += new System.EventHandler(this.EditButtonOnClick);
            // 
            // deleteButton
            // 
            this.deleteButton.Location = new System.Drawing.Point(293, 401);
            this.deleteButton.Name = "deleteButton";
            this.deleteButton.Size = new System.Drawing.Size(75, 23);
            this.deleteButton.TabIndex = 3;
            this.deleteButton.Text = "Delete";
            this.deleteButton.UseVisualStyleBackColor = true;
            this.deleteButton.Click += new System.EventHandler(this.DeleteButtonOnClick);
            // 
            // listViewTasks
            // 
            this.listViewTasks.AccessibleRole = System.Windows.Forms.AccessibleRole.None;
            this.listViewTasks.Alignment = System.Windows.Forms.ListViewAlignment.Default;
            this.listViewTasks.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.listViewTasks.CheckBoxes = true;
            this.listViewTasks.Cursor = System.Windows.Forms.Cursors.Default;
            this.listViewTasks.FullRowSelect = true;
            this.listViewTasks.HideSelection = false;
            this.listViewTasks.Location = new System.Drawing.Point(7, 55);
            this.listViewTasks.MultiSelect = false;
            this.listViewTasks.Name = "listViewTasks";
            this.listViewTasks.Size = new System.Drawing.Size(664, 326);
            this.listViewTasks.TabIndex = 5;
            this.listViewTasks.UseCompatibleStateImageBehavior = false;
            this.listViewTasks.View = System.Windows.Forms.View.Details;
            this.listViewTasks.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.ListViewTasksOnItemCheck);
            // 
            // comboBoxSort
            // 
            this.comboBoxSort.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxSort.FormattingEnabled = true;
            this.comboBoxSort.Items.AddRange(new object[] {
            "Name",
            "Deadline",
            "Priority"});
            this.comboBoxSort.Location = new System.Drawing.Point(62, 17);
            this.comboBoxSort.Name = "comboBoxSort";
            this.comboBoxSort.Size = new System.Drawing.Size(213, 21);
            this.comboBoxSort.TabIndex = 6;
            this.comboBoxSort.SelectedIndexChanged += new System.EventHandler(this.ComboBoxSortOnSelectedIndexChange);
            // 
            // sortLabel
            // 
            this.sortLabel.AutoSize = true;
            this.sortLabel.Location = new System.Drawing.Point(13, 20);
            this.sortLabel.Name = "sortLabel";
            this.sortLabel.Size = new System.Drawing.Size(43, 13);
            this.sortLabel.TabIndex = 7;
            this.sortLabel.Text = "Sort by:";
            this.sortLabel.UseWaitCursor = true;
            // 
            // textBoxFilter
            // 
            this.textBoxFilter.Location = new System.Drawing.Point(467, 17);
            this.textBoxFilter.Name = "textBoxFilter";
            this.textBoxFilter.Size = new System.Drawing.Size(173, 20);
            this.textBoxFilter.TabIndex = 8;
            this.textBoxFilter.TextChanged += new System.EventHandler(this.TextBoxFilterOnTextChange);
            // 
            // searchIcon
            // 
            this.searchIcon.BackColor = System.Drawing.SystemColors.Window;
            this.searchIcon.Image = global::WindowsFormsApp1.Properties.Resources.search_icon;
            this.searchIcon.Location = new System.Drawing.Point(646, 13);
            this.searchIcon.Name = "searchIcon";
            this.searchIcon.Size = new System.Drawing.Size(25, 25);
            this.searchIcon.TabIndex = 11;
            this.searchIcon.TabStop = false;
            this.searchIcon.Click += new System.EventHandler(this.SearchIconOnClick);
            // 
            // checkBoxUncompleted
            // 
            this.checkBoxUncompleted.AutoSize = true;
            this.checkBoxUncompleted.Location = new System.Drawing.Point(305, 19);
            this.checkBoxUncompleted.Name = "checkBoxUncompleted";
            this.checkBoxUncompleted.Size = new System.Drawing.Size(139, 17);
            this.checkBoxUncompleted.TabIndex = 12;
            this.checkBoxUncompleted.Text = "Show uncompleted only";
            this.checkBoxUncompleted.UseVisualStyleBackColor = true;
            this.checkBoxUncompleted.CheckedChanged += new System.EventHandler(this.CheckBoxUncompletedOnCheckedChange);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.ClientSize = new System.Drawing.Size(684, 461);
            this.Controls.Add(this.checkBoxUncompleted);
            this.Controls.Add(this.searchIcon);
            this.Controls.Add(this.textBoxFilter);
            this.Controls.Add(this.sortLabel);
            this.Controls.Add(this.comboBoxSort);
            this.Controls.Add(this.deleteButton);
            this.Controls.Add(this.listViewTasks);
            this.Controls.Add(this.addButton);
            this.Controls.Add(this.editButton);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximumSize = new System.Drawing.Size(700, 500);
            this.MinimumSize = new System.Drawing.Size(700, 500);
            this.Name = "Form1";
            this.Padding = new System.Windows.Forms.Padding(10);
            this.Text = "Task Manager";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.searchIcon)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button addButton;
        private System.Windows.Forms.Button editButton;
        private System.Windows.Forms.Button deleteButton;
        private System.Windows.Forms.ListView listViewTasks;
        private System.Windows.Forms.ComboBox comboBoxSort;
        private System.Windows.Forms.Label sortLabel;
        private System.Windows.Forms.TextBox textBoxFilter;
        private System.Windows.Forms.PictureBox searchIcon;
        private System.Windows.Forms.CheckBox checkBoxUncompleted;
    }
}


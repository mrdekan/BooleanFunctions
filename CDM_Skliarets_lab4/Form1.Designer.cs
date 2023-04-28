﻿namespace BooleanFunctions
{
	partial class Form1
	{
		/// <summary>
		///  Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		///  Clean up any resources being used.
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
		///  Required method for Designer support - do not modify
		///  the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.table = new System.Windows.Forms.DataGridView();
			this.submit = new System.Windows.Forms.Button();
			this.input = new System.Windows.Forms.TextBox();
			this.error_label = new System.Windows.Forms.Label();
			this.conjunction = new System.Windows.Forms.Button();
			this.disjunction = new System.Windows.Forms.Button();
			this.info = new System.Windows.Forms.Label();
			this.debug_label = new System.Windows.Forms.Label();
			this.random = new System.Windows.Forms.Button();
			((System.ComponentModel.ISupportInitialize)(this.table)).BeginInit();
			this.SuspendLayout();
			// 
			// table
			// 
			this.table.AllowUserToAddRows = false;
			this.table.AllowUserToDeleteRows = false;
			this.table.AllowUserToResizeColumns = false;
			this.table.AllowUserToResizeRows = false;
			this.table.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
			this.table.ColumnHeadersHeight = 20;
			this.table.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
			this.table.ColumnHeadersVisible = false;
			this.table.Location = new System.Drawing.Point(12, 103);
			this.table.Name = "table";
			this.table.RowHeadersVisible = false;
			this.table.RowHeadersWidth = 20;
			this.table.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
			this.table.RowTemplate.Height = 25;
			this.table.Size = new System.Drawing.Size(441, 448);
			this.table.TabIndex = 0;
			this.table.SelectionChanged += new System.EventHandler(this.table_SelectionChanged);
			// 
			// submit
			// 
			this.submit.BackColor = System.Drawing.SystemColors.ControlDarkDark;
			this.submit.Enabled = false;
			this.submit.FlatAppearance.BorderSize = 0;
			this.submit.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.submit.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
			this.submit.ForeColor = System.Drawing.SystemColors.HighlightText;
			this.submit.Location = new System.Drawing.Point(459, 12);
			this.submit.Name = "submit";
			this.submit.Size = new System.Drawing.Size(83, 29);
			this.submit.TabIndex = 19;
			this.submit.Text = "Submit";
			this.submit.UseVisualStyleBackColor = false;
			this.submit.Click += new System.EventHandler(this.submit_Click);
			// 
			// input
			// 
			this.input.BorderStyle = System.Windows.Forms.BorderStyle.None;
			this.input.Font = new System.Drawing.Font("Segoe UI", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
			this.input.Location = new System.Drawing.Point(12, 12);
			this.input.Name = "input";
			this.input.Size = new System.Drawing.Size(441, 29);
			this.input.TabIndex = 18;
			this.input.TextChanged += new System.EventHandler(this.input_TextChanged);
			// 
			// error_label
			// 
			this.error_label.AutoSize = true;
			this.error_label.ForeColor = System.Drawing.Color.Red;
			this.error_label.Location = new System.Drawing.Point(12, 44);
			this.error_label.Name = "error_label";
			this.error_label.Size = new System.Drawing.Size(0, 15);
			this.error_label.TabIndex = 20;
			// 
			// conjunction
			// 
			this.conjunction.BackColor = System.Drawing.SystemColors.ControlDarkDark;
			this.conjunction.FlatAppearance.BorderSize = 0;
			this.conjunction.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.conjunction.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
			this.conjunction.ForeColor = System.Drawing.SystemColors.HighlightText;
			this.conjunction.Location = new System.Drawing.Point(12, 68);
			this.conjunction.Name = "conjunction";
			this.conjunction.Size = new System.Drawing.Size(83, 29);
			this.conjunction.TabIndex = 21;
			this.conjunction.Text = "∧";
			this.conjunction.UseVisualStyleBackColor = false;
			this.conjunction.Click += new System.EventHandler(this.conjunction_Click);
			// 
			// disjunction
			// 
			this.disjunction.BackColor = System.Drawing.SystemColors.ControlDarkDark;
			this.disjunction.FlatAppearance.BorderSize = 0;
			this.disjunction.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.disjunction.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
			this.disjunction.ForeColor = System.Drawing.SystemColors.HighlightText;
			this.disjunction.Location = new System.Drawing.Point(101, 68);
			this.disjunction.Name = "disjunction";
			this.disjunction.Size = new System.Drawing.Size(83, 29);
			this.disjunction.TabIndex = 22;
			this.disjunction.Text = "∨";
			this.disjunction.UseVisualStyleBackColor = false;
			this.disjunction.Click += new System.EventHandler(this.disjunction_Click);
			// 
			// info
			// 
			this.info.AutoSize = true;
			this.info.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
			this.info.Location = new System.Drawing.Point(190, 55);
			this.info.Name = "info";
			this.info.Size = new System.Drawing.Size(64, 42);
			this.info.TabIndex = 23;
			this.info.Text = "*, &  = ∧\r\n+, | = ∨";
			// 
			// debug_label
			// 
			this.debug_label.AutoSize = true;
			this.debug_label.Location = new System.Drawing.Point(487, 123);
			this.debug_label.Name = "debug_label";
			this.debug_label.Size = new System.Drawing.Size(42, 15);
			this.debug_label.TabIndex = 24;
			this.debug_label.Text = "Debug";
			// 
			// random
			// 
			this.random.BackColor = System.Drawing.SystemColors.ControlDarkDark;
			this.random.FlatAppearance.BorderSize = 0;
			this.random.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.random.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
			this.random.ForeColor = System.Drawing.SystemColors.HighlightText;
			this.random.Location = new System.Drawing.Point(548, 12);
			this.random.Name = "random";
			this.random.Size = new System.Drawing.Size(83, 29);
			this.random.TabIndex = 25;
			this.random.Text = "Random";
			this.random.UseVisualStyleBackColor = false;
			this.random.Click += new System.EventHandler(this.random_Click);
			// 
			// Form1
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(800, 563);
			this.Controls.Add(this.random);
			this.Controls.Add(this.debug_label);
			this.Controls.Add(this.info);
			this.Controls.Add(this.disjunction);
			this.Controls.Add(this.conjunction);
			this.Controls.Add(this.error_label);
			this.Controls.Add(this.submit);
			this.Controls.Add(this.input);
			this.Controls.Add(this.table);
			this.Name = "Form1";
			this.Text = "Form1";
			((System.ComponentModel.ISupportInitialize)(this.table)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private DataGridView table;
		private Button submit;
		private TextBox input;
		private Label error_label;
		private Button conjunction;
		private Button disjunction;
		private Label info;
		private Label debug_label;
		private Button random;
	}
}
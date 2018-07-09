namespace OpenStatesGraphCS
{
    partial class OpenStatesGraphTestFrm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(OpenStatesGraphTestFrm));
            this.btnTestBills = new System.Windows.Forms.Button();
            this.btnTestJurisdictions = new System.Windows.Forms.Button();
            this.rtbResults = new System.Windows.Forms.RichTextBox();
            this.btnTestJurisLeges = new System.Windows.Forms.Button();
            this.btnTestPeople = new System.Windows.Forms.Button();
            this.btnTestPersonByName = new System.Windows.Forms.Button();
            this.btnTestPeopleByOrgID = new System.Windows.Forms.Button();
            this.btnTestBillsBySubject = new System.Windows.Forms.Button();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.lblSubject = new System.Windows.Forms.Label();
            this.txtSubject = new System.Windows.Forms.TextBox();
            this.dateTimePickerBills = new System.Windows.Forms.DateTimePicker();
            this.rtbBills = new System.Windows.Forms.RichTextBox();
            this.btnShowBills = new System.Windows.Forms.Button();
            this.lbTexasSessions = new System.Windows.Forms.ListBox();
            this.btnShowTexasSessions = new System.Windows.Forms.Button();
            this.lbHouseLegislators = new System.Windows.Forms.ListBox();
            this.btnShowTexasHouseLegislators = new System.Windows.Forms.Button();
            this.btnShowTexasSenateLegislators = new System.Windows.Forms.Button();
            this.lbSenateLegislators = new System.Windows.Forms.ListBox();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnTestBills
            // 
            this.btnTestBills.Location = new System.Drawing.Point(21, 35);
            this.btnTestBills.Name = "btnTestBills";
            this.btnTestBills.Size = new System.Drawing.Size(152, 23);
            this.btnTestBills.TabIndex = 0;
            this.btnTestBills.Text = "Test Bills";
            this.btnTestBills.UseVisualStyleBackColor = true;
            this.btnTestBills.Click += new System.EventHandler(this.btnTestBills_Click);
            // 
            // btnTestJurisdictions
            // 
            this.btnTestJurisdictions.Location = new System.Drawing.Point(21, 64);
            this.btnTestJurisdictions.Name = "btnTestJurisdictions";
            this.btnTestJurisdictions.Size = new System.Drawing.Size(152, 23);
            this.btnTestJurisdictions.TabIndex = 1;
            this.btnTestJurisdictions.Text = "Test States";
            this.btnTestJurisdictions.UseVisualStyleBackColor = true;
            this.btnTestJurisdictions.Click += new System.EventHandler(this.btnTestJurisdictions_Click);
            // 
            // rtbResults
            // 
            this.rtbResults.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.rtbResults.Location = new System.Drawing.Point(203, 26);
            this.rtbResults.Name = "rtbResults";
            this.rtbResults.Size = new System.Drawing.Size(495, 267);
            this.rtbResults.TabIndex = 2;
            this.rtbResults.Text = "";
            // 
            // btnTestJurisLeges
            // 
            this.btnTestJurisLeges.Location = new System.Drawing.Point(21, 93);
            this.btnTestJurisLeges.Name = "btnTestJurisLeges";
            this.btnTestJurisLeges.Size = new System.Drawing.Size(152, 23);
            this.btnTestJurisLeges.TabIndex = 3;
            this.btnTestJurisLeges.Text = "Test State Leges";
            this.btnTestJurisLeges.UseVisualStyleBackColor = true;
            this.btnTestJurisLeges.Click += new System.EventHandler(this.btnTestJurisLeges_Click);
            // 
            // btnTestPeople
            // 
            this.btnTestPeople.Location = new System.Drawing.Point(21, 122);
            this.btnTestPeople.Name = "btnTestPeople";
            this.btnTestPeople.Size = new System.Drawing.Size(152, 23);
            this.btnTestPeople.TabIndex = 4;
            this.btnTestPeople.Text = "Test People";
            this.btnTestPeople.UseVisualStyleBackColor = true;
            this.btnTestPeople.Click += new System.EventHandler(this.btnTestPeople_Click);
            // 
            // btnTestPersonByName
            // 
            this.btnTestPersonByName.Location = new System.Drawing.Point(21, 180);
            this.btnTestPersonByName.Name = "btnTestPersonByName";
            this.btnTestPersonByName.Size = new System.Drawing.Size(152, 23);
            this.btnTestPersonByName.TabIndex = 6;
            this.btnTestPersonByName.Text = "Test Person By Name";
            this.btnTestPersonByName.UseVisualStyleBackColor = true;
            this.btnTestPersonByName.Click += new System.EventHandler(this.btnTestPersonByName_Click);
            // 
            // btnTestPeopleByOrgID
            // 
            this.btnTestPeopleByOrgID.Location = new System.Drawing.Point(21, 151);
            this.btnTestPeopleByOrgID.Name = "btnTestPeopleByOrgID";
            this.btnTestPeopleByOrgID.Size = new System.Drawing.Size(152, 23);
            this.btnTestPeopleByOrgID.TabIndex = 7;
            this.btnTestPeopleByOrgID.Text = "Test People by OrgID";
            this.btnTestPeopleByOrgID.UseVisualStyleBackColor = true;
            this.btnTestPeopleByOrgID.Click += new System.EventHandler(this.btnTestPeopleByOrgID_Click);
            // 
            // btnTestBillsBySubject
            // 
            this.btnTestBillsBySubject.Location = new System.Drawing.Point(21, 209);
            this.btnTestBillsBySubject.Name = "btnTestBillsBySubject";
            this.btnTestBillsBySubject.Size = new System.Drawing.Size(152, 23);
            this.btnTestBillsBySubject.TabIndex = 8;
            this.btnTestBillsBySubject.Text = "Test Bills by Subject";
            this.btnTestBillsBySubject.UseVisualStyleBackColor = true;
            this.btnTestBillsBySubject.Click += new System.EventHandler(this.btnTestBillsBySubject_Click);
            // 
            // splitContainer1
            // 
            this.splitContainer1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.splitContainer1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.splitContainer1.Location = new System.Drawing.Point(12, 12);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.rtbResults);
            this.splitContainer1.Panel1.Controls.Add(this.btnTestBillsBySubject);
            this.splitContainer1.Panel1.Controls.Add(this.btnTestBills);
            this.splitContainer1.Panel1.Controls.Add(this.btnTestPeopleByOrgID);
            this.splitContainer1.Panel1.Controls.Add(this.btnTestJurisdictions);
            this.splitContainer1.Panel1.Controls.Add(this.btnTestPersonByName);
            this.splitContainer1.Panel1.Controls.Add(this.btnTestJurisLeges);
            this.splitContainer1.Panel1.Controls.Add(this.btnTestPeople);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.lblSubject);
            this.splitContainer1.Panel2.Controls.Add(this.txtSubject);
            this.splitContainer1.Panel2.Controls.Add(this.dateTimePickerBills);
            this.splitContainer1.Panel2.Controls.Add(this.rtbBills);
            this.splitContainer1.Panel2.Controls.Add(this.btnShowBills);
            this.splitContainer1.Panel2.Controls.Add(this.lbTexasSessions);
            this.splitContainer1.Panel2.Controls.Add(this.btnShowTexasSessions);
            this.splitContainer1.Panel2.Controls.Add(this.lbHouseLegislators);
            this.splitContainer1.Panel2.Controls.Add(this.btnShowTexasHouseLegislators);
            this.splitContainer1.Panel2.Controls.Add(this.btnShowTexasSenateLegislators);
            this.splitContainer1.Panel2.Controls.Add(this.lbSenateLegislators);
            this.splitContainer1.Size = new System.Drawing.Size(744, 683);
            this.splitContainer1.SplitterDistance = 332;
            this.splitContainer1.TabIndex = 9;
            // 
            // lblSubject
            // 
            this.lblSubject.AutoSize = true;
            this.lblSubject.Location = new System.Drawing.Point(263, 176);
            this.lblSubject.Name = "lblSubject";
            this.lblSubject.Size = new System.Drawing.Size(26, 13);
            this.lblSubject.TabIndex = 11;
            this.lblSubject.Text = "subj";
            // 
            // txtSubject
            // 
            this.txtSubject.Location = new System.Drawing.Point(295, 173);
            this.txtSubject.Name = "txtSubject";
            this.txtSubject.Size = new System.Drawing.Size(177, 20);
            this.txtSubject.TabIndex = 10;
            // 
            // dateTimePickerBills
            // 
            this.dateTimePickerBills.Location = new System.Drawing.Point(34, 174);
            this.dateTimePickerBills.Name = "dateTimePickerBills";
            this.dateTimePickerBills.Size = new System.Drawing.Size(200, 20);
            this.dateTimePickerBills.TabIndex = 9;
            // 
            // rtbBills
            // 
            this.rtbBills.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.rtbBills.Location = new System.Drawing.Point(34, 207);
            this.rtbBills.Name = "rtbBills";
            this.rtbBills.Size = new System.Drawing.Size(672, 127);
            this.rtbBills.TabIndex = 7;
            this.rtbBills.Text = "";
            // 
            // btnShowBills
            // 
            this.btnShowBills.Location = new System.Drawing.Point(499, 174);
            this.btnShowBills.Name = "btnShowBills";
            this.btnShowBills.Size = new System.Drawing.Size(207, 23);
            this.btnShowBills.TabIndex = 6;
            this.btnShowBills.Text = "Show Texas Bills";
            this.btnShowBills.UseVisualStyleBackColor = true;
            this.btnShowBills.Click += new System.EventHandler(this.btnShowBills_Click);
            // 
            // lbTexasSessions
            // 
            this.lbTexasSessions.FormattingEnabled = true;
            this.lbTexasSessions.Location = new System.Drawing.Point(499, 65);
            this.lbTexasSessions.Name = "lbTexasSessions";
            this.lbTexasSessions.Size = new System.Drawing.Size(207, 95);
            this.lbTexasSessions.TabIndex = 5;
            // 
            // btnShowTexasSessions
            // 
            this.btnShowTexasSessions.Location = new System.Drawing.Point(499, 27);
            this.btnShowTexasSessions.Name = "btnShowTexasSessions";
            this.btnShowTexasSessions.Size = new System.Drawing.Size(207, 23);
            this.btnShowTexasSessions.TabIndex = 4;
            this.btnShowTexasSessions.Text = "Show Texas Sessions";
            this.btnShowTexasSessions.UseVisualStyleBackColor = true;
            this.btnShowTexasSessions.Click += new System.EventHandler(this.btnShowTexasSessions_Click);
            // 
            // lbHouseLegislators
            // 
            this.lbHouseLegislators.FormattingEnabled = true;
            this.lbHouseLegislators.Location = new System.Drawing.Point(265, 65);
            this.lbHouseLegislators.Name = "lbHouseLegislators";
            this.lbHouseLegislators.Size = new System.Drawing.Size(207, 95);
            this.lbHouseLegislators.TabIndex = 3;
            // 
            // btnShowTexasHouseLegislators
            // 
            this.btnShowTexasHouseLegislators.Location = new System.Drawing.Point(265, 27);
            this.btnShowTexasHouseLegislators.Name = "btnShowTexasHouseLegislators";
            this.btnShowTexasHouseLegislators.Size = new System.Drawing.Size(207, 23);
            this.btnShowTexasHouseLegislators.TabIndex = 2;
            this.btnShowTexasHouseLegislators.Text = "Show Texas House Legislators";
            this.btnShowTexasHouseLegislators.UseVisualStyleBackColor = true;
            this.btnShowTexasHouseLegislators.Click += new System.EventHandler(this.btnShowTexasHouseLegislators_Click);
            // 
            // btnShowTexasSenateLegislators
            // 
            this.btnShowTexasSenateLegislators.Location = new System.Drawing.Point(34, 27);
            this.btnShowTexasSenateLegislators.Name = "btnShowTexasSenateLegislators";
            this.btnShowTexasSenateLegislators.Size = new System.Drawing.Size(207, 23);
            this.btnShowTexasSenateLegislators.TabIndex = 1;
            this.btnShowTexasSenateLegislators.Text = "Show Texas Senate Legislators";
            this.btnShowTexasSenateLegislators.UseVisualStyleBackColor = true;
            this.btnShowTexasSenateLegislators.Click += new System.EventHandler(this.btnShowTexasSenateLegislators_Click);
            // 
            // lbSenateLegislators
            // 
            this.lbSenateLegislators.FormattingEnabled = true;
            this.lbSenateLegislators.Location = new System.Drawing.Point(34, 65);
            this.lbSenateLegislators.Name = "lbSenateLegislators";
            this.lbSenateLegislators.Size = new System.Drawing.Size(207, 95);
            this.lbSenateLegislators.TabIndex = 0;
            // 
            // OpenStatesGraphTestFrm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(768, 707);
            this.Controls.Add(this.splitContainer1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "OpenStatesGraphTestFrm";
            this.Text = "Open States GraphQL Test";
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnTestBills;
        private System.Windows.Forms.Button btnTestJurisdictions;
        private System.Windows.Forms.RichTextBox rtbResults;
        private System.Windows.Forms.Button btnTestJurisLeges;
        private System.Windows.Forms.Button btnTestPeople;
        private System.Windows.Forms.Button btnTestPersonByName;
        private System.Windows.Forms.Button btnTestPeopleByOrgID;
        private System.Windows.Forms.Button btnTestBillsBySubject;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.Button btnShowTexasSenateLegislators;
        private System.Windows.Forms.ListBox lbSenateLegislators;
        private System.Windows.Forms.ListBox lbHouseLegislators;
        private System.Windows.Forms.Button btnShowTexasHouseLegislators;
        private System.Windows.Forms.ListBox lbTexasSessions;
        private System.Windows.Forms.Button btnShowTexasSessions;
        private System.Windows.Forms.DateTimePicker dateTimePickerBills;
        private System.Windows.Forms.RichTextBox rtbBills;
        private System.Windows.Forms.Button btnShowBills;
        private System.Windows.Forms.Label lblSubject;
        private System.Windows.Forms.TextBox txtSubject;
    }
}


namespace SERP
{
    partial class PopNoticeForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PopNoticeForm));
            this.panelControl1 = new DevExpress.XtraEditors.PanelControl();
            this.btn_Pre = new DevExpress.XtraEditors.SimpleButton();
            this.btn_Post = new DevExpress.XtraEditors.SimpleButton();
            this.btn_Close = new DevExpress.XtraEditors.SimpleButton();
            this.btn_DownLoad = new DevExpress.XtraEditors.SimpleButton();
            this.txt_File = new DevExpress.XtraEditors.ButtonEdit();
            this.labelControl4 = new DevExpress.XtraEditors.LabelControl();
            this.txt_Title = new DevExpress.XtraEditors.TextEdit();
            this.labelControl15 = new DevExpress.XtraEditors.LabelControl();
            this.txt_KindNM = new DevExpress.XtraEditors.TextEdit();
            this.txt_Kind = new DevExpress.XtraEditors.ButtonEdit();
            this.labelControl3 = new DevExpress.XtraEditors.LabelControl();
            this.dt_To = new DevExpress.XtraEditors.DateEdit();
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.dt_From = new DevExpress.XtraEditors.DateEdit();
            this.labelControl9 = new DevExpress.XtraEditors.LabelControl();
            this.Memo_Notice = new DevExpress.XtraEditors.MemoEdit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).BeginInit();
            this.panelControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txt_File.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txt_Title.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txt_KindNM.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txt_Kind.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dt_To.Properties.CalendarTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dt_To.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dt_From.Properties.CalendarTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dt_From.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Memo_Notice.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // panelControl1
            // 
            this.panelControl1.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.Simple;
            this.panelControl1.Controls.Add(this.btn_Pre);
            this.panelControl1.Controls.Add(this.btn_Post);
            this.panelControl1.Controls.Add(this.btn_Close);
            this.panelControl1.Controls.Add(this.btn_DownLoad);
            this.panelControl1.Controls.Add(this.txt_File);
            this.panelControl1.Controls.Add(this.labelControl4);
            this.panelControl1.Controls.Add(this.txt_Title);
            this.panelControl1.Controls.Add(this.labelControl15);
            this.panelControl1.Controls.Add(this.txt_KindNM);
            this.panelControl1.Controls.Add(this.txt_Kind);
            this.panelControl1.Controls.Add(this.labelControl3);
            this.panelControl1.Controls.Add(this.dt_To);
            this.panelControl1.Controls.Add(this.labelControl1);
            this.panelControl1.Controls.Add(this.dt_From);
            this.panelControl1.Controls.Add(this.labelControl9);
            this.panelControl1.Controls.Add(this.Memo_Notice);
            this.panelControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelControl1.Location = new System.Drawing.Point(0, 0);
            this.panelControl1.Name = "panelControl1";
            this.panelControl1.Size = new System.Drawing.Size(919, 358);
            this.panelControl1.TabIndex = 74;
            // 
            // btn_Pre
            // 
            this.btn_Pre.ImageOptions.Location = DevExpress.XtraEditors.ImageLocation.MiddleRight;
            this.btn_Pre.ImageOptions.SvgImage = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("btn_Pre.ImageOptions.SvgImage")));
            this.btn_Pre.ImageOptions.SvgImageSize = new System.Drawing.Size(25, 25);
            this.btn_Pre.Location = new System.Drawing.Point(812, 14);
            this.btn_Pre.Name = "btn_Pre";
            this.btn_Pre.Size = new System.Drawing.Size(75, 24);
            this.btn_Pre.TabIndex = 89;
            this.btn_Pre.Text = "다음";
            this.btn_Pre.Click += new System.EventHandler(this.btn_Pre_Click);
            // 
            // btn_Post
            // 
            this.btn_Post.ImageOptions.Location = DevExpress.XtraEditors.ImageLocation.MiddleLeft;
            this.btn_Post.ImageOptions.SvgImage = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("btn_Post.ImageOptions.SvgImage")));
            this.btn_Post.ImageOptions.SvgImageSize = new System.Drawing.Size(25, 25);
            this.btn_Post.Location = new System.Drawing.Point(731, 14);
            this.btn_Post.Name = "btn_Post";
            this.btn_Post.Size = new System.Drawing.Size(75, 24);
            this.btn_Post.TabIndex = 88;
            this.btn_Post.Text = "이전";
            this.btn_Post.Click += new System.EventHandler(this.btn_Post_Click);
            // 
            // btn_Close
            // 
            this.btn_Close.ImageOptions.Location = DevExpress.XtraEditors.ImageLocation.MiddleLeft;
            this.btn_Close.ImageOptions.SvgImage = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("btn_Close.ImageOptions.SvgImage")));
            this.btn_Close.ImageOptions.SvgImageSize = new System.Drawing.Size(25, 25);
            this.btn_Close.Location = new System.Drawing.Point(807, 320);
            this.btn_Close.Name = "btn_Close";
            this.btn_Close.Size = new System.Drawing.Size(80, 24);
            this.btn_Close.TabIndex = 87;
            this.btn_Close.Text = "닫기";
            this.btn_Close.Click += new System.EventHandler(this.btn_Close_Click);
            // 
            // btn_DownLoad
            // 
            this.btn_DownLoad.ImageOptions.Location = DevExpress.XtraEditors.ImageLocation.MiddleRight;
            this.btn_DownLoad.ImageOptions.SvgImage = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("btn_DownLoad.ImageOptions.SvgImage")));
            this.btn_DownLoad.ImageOptions.SvgImageSize = new System.Drawing.Size(18, 18);
            this.btn_DownLoad.Location = new System.Drawing.Point(807, 290);
            this.btn_DownLoad.Name = "btn_DownLoad";
            this.btn_DownLoad.Size = new System.Drawing.Size(80, 24);
            this.btn_DownLoad.TabIndex = 86;
            this.btn_DownLoad.Text = "파일다운";
            this.btn_DownLoad.Click += new System.EventHandler(this.btn_DownLoad_Click);
            // 
            // txt_File
            // 
            this.txt_File.Location = new System.Drawing.Point(133, 290);
            this.txt_File.Name = "txt_File";
            this.txt_File.Properties.AutoHeight = false;
            this.txt_File.Properties.ReadOnly = true;
            this.txt_File.Properties.UseReadOnlyAppearance = false;
            this.txt_File.Size = new System.Drawing.Size(668, 24);
            this.txt_File.TabIndex = 84;
            // 
            // labelControl4
            // 
            this.labelControl4.Appearance.Font = new System.Drawing.Font("나눔바른고딕", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.labelControl4.Appearance.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(80)))), ((int)(((byte)(80)))), ((int)(((byte)(80)))));
            this.labelControl4.Appearance.Options.UseFont = true;
            this.labelControl4.Appearance.Options.UseForeColor = true;
            this.labelControl4.Appearance.Options.UseTextOptions = true;
            this.labelControl4.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;
            this.labelControl4.Location = new System.Drawing.Point(33, 293);
            this.labelControl4.Name = "labelControl4";
            this.labelControl4.Size = new System.Drawing.Size(55, 17);
            this.labelControl4.TabIndex = 85;
            this.labelControl4.Text = "파일 첨부";
            // 
            // txt_Title
            // 
            this.txt_Title.Location = new System.Drawing.Point(132, 44);
            this.txt_Title.Name = "txt_Title";
            this.txt_Title.Properties.Appearance.Font = new System.Drawing.Font("나눔바른고딕", 11F);
            this.txt_Title.Properties.Appearance.Options.UseFont = true;
            this.txt_Title.Properties.ReadOnly = true;
            this.txt_Title.Properties.UseReadOnlyAppearance = false;
            this.txt_Title.Size = new System.Drawing.Size(755, 24);
            this.txt_Title.TabIndex = 79;
            // 
            // labelControl15
            // 
            this.labelControl15.Appearance.Font = new System.Drawing.Font("나눔바른고딕", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.labelControl15.Appearance.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(44)))), ((int)(((byte)(85)))), ((int)(((byte)(152)))));
            this.labelControl15.Appearance.Options.UseFont = true;
            this.labelControl15.Appearance.Options.UseForeColor = true;
            this.labelControl15.Location = new System.Drawing.Point(32, 48);
            this.labelControl15.Name = "labelControl15";
            this.labelControl15.Size = new System.Drawing.Size(68, 17);
            this.labelControl15.TabIndex = 83;
            this.labelControl15.Text = "* 공지제목";
            // 
            // txt_KindNM
            // 
            this.txt_KindNM.Enabled = false;
            this.txt_KindNM.Location = new System.Drawing.Point(610, 14);
            this.txt_KindNM.Name = "txt_KindNM";
            this.txt_KindNM.Properties.Appearance.BackColor = System.Drawing.Color.LightGray;
            this.txt_KindNM.Properties.Appearance.Font = new System.Drawing.Font("나눔바른고딕", 11F);
            this.txt_KindNM.Properties.Appearance.Options.UseBackColor = true;
            this.txt_KindNM.Properties.Appearance.Options.UseFont = true;
            this.txt_KindNM.Properties.ReadOnly = true;
            this.txt_KindNM.Properties.UseReadOnlyAppearance = false;
            this.txt_KindNM.Size = new System.Drawing.Size(115, 24);
            this.txt_KindNM.TabIndex = 82;
            // 
            // txt_Kind
            // 
            this.txt_Kind.EditValue = "";
            this.txt_Kind.Location = new System.Drawing.Point(524, 14);
            this.txt_Kind.Name = "txt_Kind";
            this.txt_Kind.Properties.Appearance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(252)))), ((int)(((byte)(247)))), ((int)(((byte)(182)))));
            this.txt_Kind.Properties.Appearance.Font = new System.Drawing.Font("나눔바른고딕", 11F);
            this.txt_Kind.Properties.Appearance.Options.UseBackColor = true;
            this.txt_Kind.Properties.Appearance.Options.UseFont = true;
            this.txt_Kind.Properties.ReadOnly = true;
            this.txt_Kind.Properties.UseReadOnlyAppearance = false;
            this.txt_Kind.Size = new System.Drawing.Size(80, 24);
            this.txt_Kind.TabIndex = 78;
            this.txt_Kind.EditValueChanged += new System.EventHandler(this.txt_Kind_EditValueChanged);
            // 
            // labelControl3
            // 
            this.labelControl3.Appearance.Font = new System.Drawing.Font("나눔바른고딕", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.labelControl3.Appearance.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(44)))), ((int)(((byte)(85)))), ((int)(((byte)(152)))));
            this.labelControl3.Appearance.Options.UseFont = true;
            this.labelControl3.Appearance.Options.UseForeColor = true;
            this.labelControl3.Location = new System.Drawing.Point(440, 18);
            this.labelControl3.Name = "labelControl3";
            this.labelControl3.Size = new System.Drawing.Size(40, 17);
            this.labelControl3.TabIndex = 81;
            this.labelControl3.Text = "* 구분";
            // 
            // dt_To
            // 
            this.dt_To.EditValue = null;
            this.dt_To.Location = new System.Drawing.Point(266, 14);
            this.dt_To.Name = "dt_To";
            this.dt_To.Properties.Appearance.Font = new System.Drawing.Font("나눔바른고딕", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.dt_To.Properties.Appearance.Options.UseFont = true;
            this.dt_To.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.dt_To.Properties.CalendarTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.dt_To.Properties.ReadOnly = true;
            this.dt_To.Properties.UseReadOnlyAppearance = false;
            this.dt_To.Size = new System.Drawing.Size(117, 24);
            this.dt_To.TabIndex = 76;
            // 
            // labelControl1
            // 
            this.labelControl1.Location = new System.Drawing.Point(253, 19);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(9, 15);
            this.labelControl1.TabIndex = 80;
            this.labelControl1.Text = "~";
            // 
            // dt_From
            // 
            this.dt_From.EditValue = null;
            this.dt_From.Location = new System.Drawing.Point(132, 14);
            this.dt_From.Name = "dt_From";
            this.dt_From.Properties.Appearance.Font = new System.Drawing.Font("나눔바른고딕", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.dt_From.Properties.Appearance.Options.UseFont = true;
            this.dt_From.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.dt_From.Properties.CalendarTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.dt_From.Properties.ReadOnly = true;
            this.dt_From.Properties.UseReadOnlyAppearance = false;
            this.dt_From.Size = new System.Drawing.Size(117, 24);
            this.dt_From.TabIndex = 75;
            // 
            // labelControl9
            // 
            this.labelControl9.Appearance.Font = new System.Drawing.Font("나눔바른고딕", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.labelControl9.Appearance.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(44)))), ((int)(((byte)(85)))), ((int)(((byte)(152)))));
            this.labelControl9.Appearance.Options.UseFont = true;
            this.labelControl9.Appearance.Options.UseForeColor = true;
            this.labelControl9.Location = new System.Drawing.Point(32, 18);
            this.labelControl9.Name = "labelControl9";
            this.labelControl9.Size = new System.Drawing.Size(68, 17);
            this.labelControl9.TabIndex = 77;
            this.labelControl9.Text = "* 공지기간";
            // 
            // Memo_Notice
            // 
            this.Memo_Notice.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.Memo_Notice.Location = new System.Drawing.Point(32, 74);
            this.Memo_Notice.Name = "Memo_Notice";
            this.Memo_Notice.Properties.ReadOnly = true;
            this.Memo_Notice.Properties.UseReadOnlyAppearance = false;
            this.Memo_Notice.Size = new System.Drawing.Size(855, 210);
            this.Memo_Notice.TabIndex = 74;
            // 
            // PopNoticeForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(919, 358);
            this.Controls.Add(this.panelControl1);
            this.FormBorderEffect = DevExpress.XtraEditors.FormBorderEffect.None;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "PopNoticeForm";
            this.Text = "PopNoticeForm";
            this.Load += new System.EventHandler(this.PopNoticeForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).EndInit();
            this.panelControl1.ResumeLayout(false);
            this.panelControl1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txt_File.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txt_Title.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txt_KindNM.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txt_Kind.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dt_To.Properties.CalendarTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dt_To.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dt_From.Properties.CalendarTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dt_From.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Memo_Notice.Properties)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.PanelControl panelControl1;
        private DevExpress.XtraEditors.SimpleButton btn_Pre;
        private DevExpress.XtraEditors.SimpleButton btn_Post;
        private DevExpress.XtraEditors.SimpleButton btn_Close;
        private DevExpress.XtraEditors.SimpleButton btn_DownLoad;
        private DevExpress.XtraEditors.ButtonEdit txt_File;
        private DevExpress.XtraEditors.LabelControl labelControl4;
        private DevExpress.XtraEditors.TextEdit txt_Title;
        private DevExpress.XtraEditors.LabelControl labelControl15;
        private DevExpress.XtraEditors.TextEdit txt_KindNM;
        private DevExpress.XtraEditors.ButtonEdit txt_Kind;
        private DevExpress.XtraEditors.LabelControl labelControl3;
        private DevExpress.XtraEditors.DateEdit dt_To;
        private DevExpress.XtraEditors.LabelControl labelControl1;
        private DevExpress.XtraEditors.DateEdit dt_From;
        private DevExpress.XtraEditors.LabelControl labelControl9;
        private DevExpress.XtraEditors.MemoEdit Memo_Notice;
    }
}
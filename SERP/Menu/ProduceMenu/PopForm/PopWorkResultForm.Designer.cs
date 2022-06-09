namespace SERP
{
    partial class PopWorkResultForm
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.Grid_ResultS = new SERP.GridControlEx();
            this.View_ResultS = new DevExpress.XtraGrid.Views.Grid.GridView();
            ((System.ComponentModel.ISupportInitialize)(this.Grid_ResultS)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.View_ResultS)).BeginInit();
            this.SuspendLayout();
            // 
            // Grid_ResultS
            // 
            this.Grid_ResultS.AddRowYN = false;
            this.Grid_ResultS.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Grid_ResultS.EnterYN = true;
            this.Grid_ResultS.Execl_GB = SERP.GridControlEx.Excel_GB.Update;
            this.Grid_ResultS.ExpansionCHK = false;
            this.Grid_ResultS.Location = new System.Drawing.Point(0, 0);
            this.Grid_ResultS.MainView = this.View_ResultS;
            this.Grid_ResultS.MouseWheelChk = true;
            this.Grid_ResultS.MultiSelectChk = true;
            this.Grid_ResultS.Name = "Grid_ResultS";
            this.Grid_ResultS.PopMenuChk = true;
            this.Grid_ResultS.Size = new System.Drawing.Size(850, 450);
            this.Grid_ResultS.TabIndex = 0;
            this.Grid_ResultS.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.View_ResultS});
            // 
            // View_ResultS
            // 
            this.View_ResultS.GridControl = this.Grid_ResultS;
            this.View_ResultS.Name = "View_ResultS";
            this.View_ResultS.OptionsClipboard.PasteMode = DevExpress.Export.PasteMode.Update;
            this.View_ResultS.OptionsSelection.MultiSelect = true;
            this.View_ResultS.OptionsView.ShowGroupPanel = false;
            // 
            // PopWorkResultForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(850, 450);
            this.Controls.Add(this.Grid_ResultS);
            this.Name = "PopWorkResultForm";
            this.Load += new System.EventHandler(this.PopWorkResultForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.Grid_ResultS)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.View_ResultS)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private GridControlEx Grid_ResultS;
        private DevExpress.XtraGrid.Views.Grid.GridView View_ResultS;
    }
}

using DevExpress.XtraTreeList;
using DevExpress.XtraTreeList.Columns;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SERP
{
    public static class SetUp_Tree
    {
        public static void Tree_Columns_Set(TreeList treeList, string sCaption, string sFieldName, bool visible)
        {
            TreeListColumn t_Column = new TreeListColumn();

            t_Column.Caption = sCaption;
            t_Column.FieldName = sFieldName;
            if (visible)
                t_Column.VisibleIndex = treeList.Columns.Count;
            else
                t_Column.VisibleIndex = -1;

            treeList.Columns.Add(t_Column);
        }

        public static void Tree_Columns_Key(TreeList treeList, string sKeyName, string sParentName)
        {
            treeList.KeyFieldName = sKeyName;
            treeList.ParentFieldName = sParentName;
        }
    }
}

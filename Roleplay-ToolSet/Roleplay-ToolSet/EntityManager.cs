using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RoleplayToolSet
{
    public partial class EntityManager : UserControl
    {
        private EntityCollection _entityCollection;
        public EntityCollection Entities // Used to set entities above
        {
            set
            {
                _entityCollection = value;

                // Pass down
                entityList.Entities = _entityCollection;
                entityDetailedDisplay.Entities = _entityCollection;
            }
        }

        public EntityManager()
        {
            InitializeComponent();
        }

        private void SetEntityToSelected()
        {
            entityDetailedDisplay.Entity = entityList.GetSelectedEntity();
        }

        private void EntityList_SelectionChanged(object sender, EventArgs e)
        {
            SetEntityToSelected();
        }
    }
}

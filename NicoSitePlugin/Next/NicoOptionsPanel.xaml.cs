﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using SitePlugin;
using System.ComponentModel;

namespace NicoSitePlugin.Next
{
    /// <summary>
    /// Interaction logic for NicoOptionsPanel.xaml
    /// </summary>
    public partial class NicoOptionsPanel : UserControl
    {
        public NicoOptionsPanel()
        {
            InitializeComponent();
        }
        public void SetViewModel(NicoSiteOptionsViewModel vm)
        {
            this.DataContext = vm;
        }
        public NicoSiteOptionsViewModel GetViewModel()
        {
            return (NicoSiteOptionsViewModel)this.DataContext;
        }
    }
    public class NicoSiteOptionsViewModel : INotifyPropertyChanged
    {
        public int OfficialRoomsRetrieveCount
        {
            get { return _changed.OfficialRoomsRetrieveCount; }
            set { _changed.OfficialRoomsRetrieveCount = value; }
        }
        private readonly NicoSiteOptions _origin;
        private readonly NicoSiteOptions _changed;
        internal NicoSiteOptions OriginOptions { get { return _origin; } }
        internal NicoSiteOptions ChangedOptions { get { return _changed; } }

        public NicoSiteOptionsViewModel()
        {
            //if(IsInDesigner)
            _changed = new NicoSiteOptions { OfficialRoomsRetrieveCount = 5 };

        }
        internal NicoSiteOptionsViewModel(NicoSiteOptions siteOptions)
        {
            _origin = siteOptions;
            _changed = siteOptions.Clone();
        }

        #region INotifyPropertyChanged
        [NonSerialized]
        private System.ComponentModel.PropertyChangedEventHandler _propertyChanged;
        /// <summary>
        /// 
        /// </summary>
        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged
        {
            add { _propertyChanged += value; }
            remove { _propertyChanged -= value; }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="propertyName"></param>
        protected void RaisePropertyChanged([System.Runtime.CompilerServices.CallerMemberName] string propertyName = "")
        {
            _propertyChanged?.Invoke(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
        }
        #endregion
    }
}

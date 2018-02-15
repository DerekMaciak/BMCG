// ***********************************************************************
// Assembly         : BMCGMobile
// Author           : Derek Maciak
// Created          : 01-22-2018
//
// Last Modified By : Derek Maciak
// Last Modified On : 01-22-2018
// ***********************************************************************
// <copyright file="LocalizedResources.cs" company="">
//     Copyright ©  2018
// </copyright>
// <summary></summary>
// ***********************************************************************
using System;
using System.ComponentModel;
using System.Globalization;
using System.Resources;
using Xamarin.Forms;

namespace BMCGMobile.Localization
{
    /// <summary>
    /// Class LocalizedResources.
    /// </summary>
    /// <seealso cref="System.ComponentModel.INotifyPropertyChanged" />
    public class LocalizedResources : INotifyPropertyChanged
    {
        /// <summary>
        /// The default language
        /// </summary>
        private const string DEFAULT_LANGUAGE = "en";

        /// <summary>
        /// The resource manager
        /// </summary>
        private readonly ResourceManager ResourceManager;
        /// <summary>
        /// The current culture information
        /// </summary>
        private CultureInfo CurrentCultureInfo;

        /// <summary>
        /// Gets the <see cref="System.String"/> with the specified key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns>System.String.</returns>
        public string this[string key]
        {
            get
            {
                return ResourceManager.GetString(key, CurrentCultureInfo);
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LocalizedResources"/> class.
        /// </summary>
        /// <param name="resource">The resource.</param>
        /// <param name="language">The language.</param>
        public LocalizedResources(Type resource, string language = null)
            : this(resource, new CultureInfo(language ?? DEFAULT_LANGUAGE))
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="LocalizedResources"/> class.
        /// </summary>
        /// <param name="resource">The resource.</param>
        /// <param name="cultureInfo">The culture information.</param>
        public LocalizedResources(Type resource, CultureInfo cultureInfo)
        {
            CurrentCultureInfo = cultureInfo;
            ResourceManager = new ResourceManager(resource);

            MessagingCenter.Subscribe<object, CultureChangedMessage>(this,
                string.Empty, OnCultureChanged);
        }

        /// <summary>
        /// Called when [culture changed].
        /// </summary>
        /// <param name="s">The s.</param>
        /// <param name="ccm">The CCM.</param>
        private void OnCultureChanged(object s, CultureChangedMessage ccm)
        {
            CurrentCultureInfo = ccm.NewCultureInfo;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Item"));
        }

        /// <summary>
        /// Occurs when a property value changes.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;
    }
}
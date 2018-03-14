// ***********************************************************************
// Assembly         : BMCGMobile
// Author           : Derek Maciak
// Created          : 01-22-2018
//
// Last Modified By : Derek Maciak
// Last Modified On : 01-24-2018
// ***********************************************************************
// <copyright file="EntityBase.cs" company="">
//     Copyright ©  2018
// </copyright>
// <summary></summary>
// ***********************************************************************
using BMCGMobile.Localization;
using BMCGMobile.Resources;
using Newtonsoft.Json;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace BMCGMobile.Entities
{
    /// <summary>
    /// Class EntityBase.
    /// </summary>
    /// <seealso cref="System.ComponentModel.INotifyPropertyChanged" />
    public class EntityBase : INotifyPropertyChanged
    {
        /// <summary>
        /// Gets the resources.
        /// </summary>
        /// <value>The resources.</value>
        [JsonIgnore]
        public LocalizedResources Resources
        {
            get;
            private set;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EntityBase"/> class.
        /// </summary>
        public EntityBase()
        {
            Resources = new LocalizedResources(typeof(DesciptionResource), App.CurrentLanguage);
        }

        /// <summary>
        /// Called when [property changed].
        /// </summary>
        /// <param name="property">The property.</param>
        public void OnPropertyChanged([CallerMemberName]string property = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));
        }

        /// <summary>
        /// Occurs when a property value changes.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;
    }
}
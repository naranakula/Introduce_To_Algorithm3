﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetCommsConsole.NetComms
{
    /// <summary>
    /// 指定当前的运行时环境
    /// </summary>
    public enum RuntimeEnvironment
    {
        /// <summary>
        /// Native .Net 4.0 - Default
        /// </summary>
        Native_Net4,

        /// <summary>
        /// Mono .Net 4.0
        /// </summary>
        Mono_Net4,

        /// <summary>
        /// Native .Net3.5
        /// </summary>
        Native_Net35,

        /// <summary>
        /// Mono .Net 3.5
        /// </summary>
        Mono_Net35,

        /// <summary>
        /// Native .Net 2
        /// </summary>
        Native_Net2,

        /// <summary>
        /// Mono .Net 2
        /// </summary>
        Mono_Net2,

        /// <summary>
        /// Windows Phone 7.1 (8) or Silverlight
        /// </summary>
        WindowsPhone_Silverlight,

        /// <summary>
        /// Xamarin.Android
        /// </summary>
        Xamarin_Android,

        /// <summary>
        /// Xamarin.iOS
        /// </summary>
        Xamarin_iOS,

        /// <summary>
        /// Windows RT or Windows Store 
        /// </summary>
        Windows_RT,
    }
}

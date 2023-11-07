using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Essentials;

namespace kursovaya
{
    public class ClientInfo
    {
        static public ApiRequest.Request.Client Profile { get; set; }
        static public double screenWidth { get; private set; }
        static public double screenHeight { get; private set; }
        static public double density { get; private set; }
        public ClientInfo()
        {
            // Получение размеров экрана
            var mainDisplayInfo = DeviceDisplay.MainDisplayInfo;

            screenWidth = mainDisplayInfo.Width;
            screenHeight = mainDisplayInfo.Height;
            density = mainDisplayInfo.Density;
        }
    }
}

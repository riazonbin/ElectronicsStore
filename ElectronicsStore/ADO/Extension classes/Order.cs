using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace ElectronicsStore.ADO
{
    public partial class Order
    {
        public double TotalOrderSum => (double)OrderContent.Sum(x => x.Product.Price * x.Count);

        public SolidColorBrush BackgroundColor => OrderStatus_Id == 1
            ? System.Windows.Media.Brushes.Gray : (OrderStatus_Id == 2 || OrderStatus_Id == 3)
            ? System.Windows.Media.Brushes.Blue : System.Windows.Media.Brushes.Green;

        public string OrderStatusName => App.Connection.OrderStatus.FirstOrDefault(x => x.Id == OrderStatus_Id).Name;

        public BitmapImage GenerateOrderQR()
        {
            try
            {
                // Ссылка на XL таблицу
                string soucer_xl = $"{Id}";
                // Создание переменной библиотеки QRCoder
                QRCoder.QRCodeGenerator qr = new QRCoder.QRCodeGenerator();
                // Присваеваем значиения
                QRCoder.QRCodeData data = qr.CreateQrCode(soucer_xl, QRCoder.QRCodeGenerator.ECCLevel.L);
                // переводим в Qr
                QRCoder.QRCode code = new QRCoder.QRCode(data);
                Bitmap bitmap = code.GetGraphic(100);
                /// Создание картинки
                using (MemoryStream memory = new MemoryStream())
                {
                    bitmap.Save(memory, System.Drawing.Imaging.ImageFormat.Bmp);
                    memory.Position = 0;
                    BitmapImage bitmapimage = new BitmapImage();
                    bitmapimage.BeginInit();
                    bitmapimage.StreamSource = memory;
                    bitmapimage.CacheOption = BitmapCacheOption.OnLoad;
                    bitmapimage.EndInit();
                    return bitmapimage;
                }
            }
            catch
            {
                return null;
            }
        }

        public BitmapImage OrderQR => GenerateOrderQR();

        public Visibility IsAdminMode => App.CurrentUser.Role_Id == 2 ? Visibility.Visible : Visibility.Collapsed;
    }
}

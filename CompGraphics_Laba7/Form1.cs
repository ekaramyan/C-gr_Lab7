using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Drawing2D;

namespace CompGraphics_Laba7
{
    public partial class Form1 : Form // Size Form: 1 000, 600, Size PictureBox: 975, 490
    {
        // Битовая картинка pictureBox
        Bitmap pictureBoxBitMap;
        // Битовая картинка динамического изображения
        Bitmap spriteBitMap;
        // Битовая картинка для временного хранения области экрана
        Bitmap cloneBitMap;
        // Графический контекст picturebox
        Graphics g_pictureBox;
        // Графический контекст спрайта
        Graphics g_sprite;
        int x, y; // Координаты ракеты
        int width = 300, height = 175; // Ширина и высота автобуса
        Timer timer;

        public Form1()
        {
            InitializeComponent();
        }

        // Функция рисования спрайта (корабля)
        void DrawSprite()
        {
            // Задаем красный цвет для носа ракеты
            SolidBrush myNos = new SolidBrush(Color.Red);
            // Задаем серебряный цвет для топливных баков
            SolidBrush myBak = new SolidBrush(Color.Silver);
            // Задаем белый и серый цвет для корпуса ракеты
            SolidBrush myShip = new SolidBrush(Color.White);
            SolidBrush myLine = new SolidBrush(Color.Gray);
            // Задаем желтый и оранжевый цвет для пламени
            SolidBrush myFire1 = new SolidBrush(Color.Yellow);
            SolidBrush myFire2 = new SolidBrush(Color.Orange);
            // Задаем желто-зеленый цвет для звезды
            SolidBrush myStar = new SolidBrush(Color.GreenYellow);
            // ******* 1 - Рисуем топливные баки ************
            // Рисуем два прямоугольника
            g_sprite.FillRectangle(myBak, 179, 115, 26, 175);
            g_sprite.FillRectangle(myBak, 275, 115, 26, 175);
            // Сверху каждого прямоугольника рисуем по треугольнику
            g_sprite.FillPolygon(myBak, new Point[]
                {

            new Point(115,179),new Point(100,192),
            new Point(100,192),new Point(115,205),
            new Point(115,205),new Point(115,179)

            });
            {

                g_sprite.FillPolygon(myBak, new Point[] {

            new Point(275,115),new Point(287,100),
            new Point(287,100),new Point(301,115),
            new Point(301,115),new Point(275,115)

            });


                // ************* 2 - Рисуем нос ракеты ***************
                g_sprite.FillPolygon(myNos, new Point[]
            {

            new Point(205,90),new Point(240,60),
            new Point(240,60),new Point(275,90),
            new Point(275,90),new Point(275,290),
            new Point(275,290),new Point(205,290),
            new Point(205,290),new Point(205,90)

                });
                // ******** 3 - Рисуем нижнюю часть ракеты ************
                g_sprite.FillPolygon(myLine, new Point[] {

            new Point(130,300),new Point(240,260),
            new Point(240,260),new Point(345,300),
            new Point(345,300),new Point(130,300)

            });
                // ******** 4 - Рисуем часть ракеты ниже носа **********
                g_sprite.FillPolygon(myLine, new Point[] {

            new Point(204,145),new Point(240,115),
            new Point(240,115),new Point(276,145),
            new Point(276,145),new Point(204,145)
           });
                // ********** 5 - Рисуем корпус ракеты белым цветом *****
                g_sprite.FillRectangle(myShip, 204, 145, 72, 155);
                // ******* 6 - Рисуем серую полосу на корпусе ракеты *****
                g_sprite.FillRectangle(myLine, 204, 185, 72, 50);
                // *********** 7 - Рисуем пламя из сопла ракеты *********
                // Определяем графический контейнер
                GraphicsPath myGraphicsPath = new GraphicsPath();
                Pen p = new Pen(Brushes.Red, 1);
                // Задаем координаты точек первой кривой (внутреннее пламя)
                Point[] myPointArray1 = { new Point(210, 300),
            new Point(210, 330), new Point(240, 360),
            new Point(270, 330), new Point(270, 300)};

                // Добавляем кривую в контейнер
                myGraphicsPath.AddCurve(myPointArray1);
                // Выводим внутренню часть пламени, закрашенную желтым цветом
                g_sprite.FillPath(myFire1, myGraphicsPath);
                // Задаем координаты точек второй кривой (внешнее пламя)
                Point[] myPointArray2 = { new Point(185, 300),
            new Point(185, 360), new Point(240, 430),
            new Point(295, 360), new Point(295, 300) };

                // Добавляем кривую в контейнер
                myGraphicsPath.AddCurve(myPointArray2);
            }


        }

        // Функция сохранения части изображения шириной
        void SavePart(int xt, int yt)
        {
            Rectangle cloneRect = new Rectangle(xt, yt, width, height);
            System.Drawing.Imaging.PixelFormat format =
            pictureBoxBitMap.PixelFormat;
            // Клонируем изображение, заданное прямоугольной областью
            cloneBitMap = pictureBoxBitMap.Clone(cloneRect, format);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // Создаём Bitmap для pictureBox1 и графический контекст
            pictureBox1.Image = Image.FromFile("F:\\Загрузки\\Chrome Downloads\\CompGraphics_Laba7-master\\CompGraphics_Laba7-master\\ocean.jpg");
            pictureBoxBitMap = new Bitmap(pictureBox1.Image);
            g_pictureBox = Graphics.FromImage(pictureBox1.Image);
            // Создаём Bitmap для спрайта и графический контекст
            spriteBitMap = new Bitmap(width, height);
            g_sprite = Graphics.FromImage(spriteBitMap);
            // Рисование и закраска моря
            int r = 50, iks = 50;
            while (iks <= pictureBox1.Width + r)
            {
                g_pictureBox.FillPie(Brushes.Blue, -50 + iks, 375, 50, 50, 0, -180);
                iks += 50;
            }
            // Рисуем автобус на графическом контексте g_sprite
            DrawSprite();
            // Создаём Bitmap для временного хранения части изображения
            cloneBitMap = new Bitmap(width, height);
            // Задаем начальные координаты вывода движущегося объекта
            x = 0; y = 200;
            // Сохраняем область экрана перед первым выводом объекта
            SavePart(x, y);
            // Выводим корабль на графический контекст g_pictureBox
            g_pictureBox.DrawImage(spriteBitMap, x, y);
            // Перерисовываем pictureBox1
            pictureBox1.Invalidate();
            // Создаём таймер с интервалом 100 миллисекунд
            timer = new Timer();
            timer.Interval = 100;
            timer.Tick += new EventHandler(timer1_Tick);
        }


        // Обрабатываем событие от таймера
        private void timer1_Tick(object sender, EventArgs e)
        {
            // Восстанавливаем затёртую область статического изображения
            g_pictureBox.DrawImage(cloneBitMap, x, y);
            // Изменяем координаты для следующего вывода автобуса
            x += 6;
            // Проверяем на выход изображения автобуса за правую границу
            if (x > pictureBox1.Width - 1) x = pictureBox1.Location.X;
            // Сохраняем область экрана перед первым выводом автобуса
            SavePart(x, y);
            // Выводим автобус
            g_pictureBox.DrawImage(spriteBitMap, x, y);
            // Перерисовываем pictureBox1
            pictureBox1.Invalidate();
        }
        // Включаем таймер по нажатию на кнопку
        private void button1_Click(object sender, EventArgs e)
        {
            timer.Enabled = true;
        }
    }
}


/* {
            Pen myWindows = new Pen(Color.BurlyWood, 2);
            // Определение кистей
            SolidBrush myBody = new SolidBrush(Color.Black);
            SolidBrush myHold = new SolidBrush(Color.Green);
            SolidBrush myTube = new SolidBrush(Color.Gray);
            // Рисование и закраска труб, трюма и корпуса корабля
            g_sprite.FillRectangle(myTube, 100, 0, 25, 25);
            g_sprite.FillRectangle(myTube, 150, 0, 25, 25);

            g_sprite.FillPolygon(myBody, new Point[] {
                new Point(0, 75),new Point(300, 75),
                new Point(235, 175), new Point(50, 175),
                new Point(0, 75)
            });
            g_sprite.FillRectangle(myHold, 50, 25, 175, 50);

            g_sprite.DrawEllipse(myWindows, 90, 40, 20, 20);
            g_sprite.DrawEllipse(myWindows, 130, 40, 20, 20);
            g_sprite.DrawEllipse(myWindows, 170, 40, 20, 20);
        }*/

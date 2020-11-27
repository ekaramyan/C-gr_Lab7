﻿using System;
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
        //Drawing Ship
        Graphics g_ship;
        // Графический контекст спрайта
        Graphics g_sprite;
        int x, y; // Координаты ракеты
        int width = 300, height = 175; // Ширина и высота 
        Timer timer;

        public Form1()
        {
            InitializeComponent();
        }
        void DrawShip()
        {
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
        }
        // Функция рисования спрайта (корабля)
        void DrawSprite()
        {
            // Задаем красный цвет для носа ракеты
            SolidBrush myNos = new SolidBrush(Color.Red);
            // Задаем серебряный цвет для топливных баков
            SolidBrush myBak = new SolidBrush(Color.Silver);
            // Задаем белый и серый цвет для корпуса ракеты
            SolidBrush myShip = new SolidBrush(Color.Silver);
            SolidBrush myLine = new SolidBrush(Color.Gray);
            // Задаем желтый и оранжевый цвет для пламени
            SolidBrush myFire1 = new SolidBrush(Color.Yellow);
            SolidBrush myFire2 = new SolidBrush(Color.Orange);

            { 
                //Рисуем нос ракеты
                g_sprite.FillPolygon(myNos, new Point[]
            {

            new Point(275,143),new Point(275,260),
            new Point(380,235)//,new Point(300,145),
            //new Point(250,145),new Point(300,300),
            //new Point(300,300),new Point(250,145),
            //new Point(250,145),new Point(300,300)

                });
                //Рисуем нижнюю часть ракеты
                g_sprite.FillPolygon(myLine, new Point[] {

            new Point(204,130),new Point(204,130),
            new Point(204,150),new Point(235,150),
            //new Point(120,145),new Point(120,200)

            });
                //Рисуем корпус ракеты белым цветом
               g_sprite.FillRectangle(myShip, 204, 145, 72, 155);
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
            pictureBox1.Image = Image.FromFile("F:\\Labs\\C-gr7\\ocean.jpg");
            pictureBoxBitMap = new Bitmap(pictureBox1.Image);
            g_pictureBox = Graphics.FromImage(pictureBox1.Image);
            // Создаём Bitmap для спрайта и графический контекст
            spriteBitMap = new Bitmap(width, height);
            g_sprite = Graphics.FromImage(spriteBitMap);
            g_ship = Graphics.FromImage(spriteBitMap);
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

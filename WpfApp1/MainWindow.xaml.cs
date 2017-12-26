using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Media;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Windows.Data;
//using System.Windows.Documents;
//using System.Windows.Input;
//using System.Windows.Media.Imaging;
//using System.Windows.Navigation;
//using System.Threading;


namespace WpfApp1
{
   
    public partial class MainWindow : Window
    {

        int bstate = 1;//estado el boton -1 ejecutandose 
        
        public MainWindow()
        {
            InitializeComponent();
        }
       
        private void button_Click(object sender, RoutedEventArgs e)
        {
            bstate= bstate*(-1);
            
            Click();
        }


        void Click()
        {


            if (bstate == 1)
                Iniciar.Content = "Iniciar";
            else
                Iniciar.Content = "Detener";


            string sele = ((tmp.SelectedItem as ComboBoxItem).Content).ToString();//obtiene el valor seleccionado y lo convierte a cadena 
            int compas = 1;

            switch (sele)
            {
                case "2/4":
                    compas = 2;

                    break;
                case "3/4":
                    compas = 3;

                    break;
                case "4/4":
                    compas = 4;

                    break;

            }


            int t = Int16.Parse(bpm.Text);
            t = 60000 / t;

           
            ejecutar_metro(compas,t); //reproduce los clicks

            


            
            
        }


        async void ejecutar_metro(int compas,int t)
        {
            
            SoundPlayer sound; //se declara el objeto sonido
            sound = new SoundPlayer(@"C:\Users\SNK93\source\repos\WpfApp1\WpfApp1\bin\Debug\Sonidos\tiempo_fuerte.wav");//se asigna path del audio


            Rectangle[] cajas = new Rectangle[4];//arreglo de donde se mostrara cada click

           

            int phorizontal = -400; //posicion horizontal de cada rectangulo

            for (int k = 0; k < compas; k++)//imprime todos los rectangulos 
            {
                cajas[k] = new Rectangle();
                cajas[k].Height = 70;
                cajas[k].Margin = new Thickness(phorizontal, 165, 0, 0);
                cajas[k].Stroke = Brushes.Black;
                cajas[k].Width = 80;
                monitor.Children.Add(cajas[k]);

                phorizontal += 265;
            }
            
            int aumento_din = 0;

            aumento_din = Int16.Parse(avanzar.Text);

            if (check_td.IsChecked == false)
                aumento_din = 0;
            else
                aumento_din = int.Parse(avanzar.Text);


            await Task.Delay(1);   
            check_td.IsEnabled = false;//bloquea grupo tempo dinamico mientras se reproduce 
            bloquear_grupo_td();

            switch (compas)
            {

                case 4:

                   

                    while (bstate == -1)//reproduce en los rectangulos los clicks
                    {



                        for (int k = 0; k < Int16.Parse(cada.Text); k++)
                        {
                            

                            sound.Play();
                            cajas[3].Fill = new SolidColorBrush(Colors.White);
                            cajas[0].Fill = new SolidColorBrush(Colors.Red);
                            await Task.Delay(t);
                            if (bstate == 1)
                                break;
                            
                            sound.Play();
                            cajas[0].Fill = new SolidColorBrush(Colors.White);
                            cajas[1].Fill = new SolidColorBrush(Colors.Black);
                            await Task.Delay(t);
                            if (bstate == 1)
                                break;

                            sound.Play();
                            cajas[1].Fill = new SolidColorBrush(Colors.White);
                            cajas[2].Fill = new SolidColorBrush(Colors.Black);
                            await Task.Delay(t);
                            if (bstate == 1)
                                break;

                            sound.Play();
                            cajas[2].Fill = new SolidColorBrush(Colors.White);
                            cajas[3].Fill = new SolidColorBrush(Colors.Black);
                            await Task.Delay(t);

                            if (bstate == 1)
                                break;
                        }

                        if (bstate == -1)
                        {
                            t = (Int16.Parse(bpm.Text)) + aumento_din;//aumenta en la variable t los puntos asignados
                            bpm.Text = t.ToString();//coloca en textbox el nuevo tempo
                            t = 60000 / t; //calcula los milisegundos

                        }




                    }

                    

                break;


                default:MessageBox.Show("tempo aun no programado");
                    break;




            }
            if (check_td.IsChecked == true)
                desbloquear_grupo_td();
            check_td.IsEnabled = true;
            Limpiar_cajas(cajas, compas);

        }
        

        void Limpiar_cajas(Rectangle []tempo,int compas)//coloca en blanco todas las cajas donde se muestran los beats
        {

            for (int k = 0; k < compas; k++)
                tempo[k].Fill =new SolidColorBrush(Colors.White) ;

        }
       

        private void mas_Click(object sender, RoutedEventArgs e)  //aumenta un punto en el bloque de texto de bpm
        {
            int m = Int16.Parse(bpm.Text);
            m++;
            bpm.Text = m.ToString();
        }

        private void menos_Click(object sender, RoutedEventArgs e)//disminuye un punto en el bloque de textto de bpm
        {
            int m = Int16.Parse(bpm.Text);
            m--;
            bpm.Text = m.ToString();
        }

        private void masdiez_Click(object sender, RoutedEventArgs e)//aumenta 10 puntos en el bloque de texto de bpm
        {
            int m = Int16.Parse(bpm.Text);
            m+=10;
            bpm.Text = m.ToString();
        }

        private void mascinco_Click(object sender, RoutedEventArgs e)//disminuye 5 puntos en el bloque de texto de  bpm
        {
            int m = Int16.Parse(bpm.Text);
            m+=5;
            bpm.Text = m.ToString();
        }

        private void check_td_Checked(object sender, RoutedEventArgs e)
        {
            desbloquear_grupo_td();
           
        }


        private void check_td_unchecked(object sender, RoutedEventArgs e)
        {

            bloquear_grupo_td();
   
        }

        private void bloquear_grupo_td()
        {
            avanzarlab.IsEnabled = false;
            bpmlab.IsEnabled = false;
            compaseslab.IsEnabled = false;
            avanzar.IsEnabled = false;
            cada.IsEnabled = false;
            cadalab.IsEnabled = false;


        }

        private void desbloquear_grupo_td()
        {
            avanzar.IsEnabled = true;
            cada.IsEnabled = true;
            avanzarlab.IsEnabled = true;
            bpmlab.IsEnabled = true;
            compaseslab.IsEnabled = true;
            cadalab.IsEnabled = true;



        }
    }

}

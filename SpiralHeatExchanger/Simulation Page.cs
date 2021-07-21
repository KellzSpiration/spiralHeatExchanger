using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SpiralHeatExchanger
{
    public partial class Simulation_Page : Form
    {
        public Simulation_Page()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // Initialization of the hot fluid physical properties
            double HTin = double.Parse(txtHTin.Text.ToString());
            double HTout = double.Parse(txtHTout.Text.ToString());
            double CpHot = double.Parse(txtCpHot.Text.ToString());
            double HotFr = double.Parse(txtHotFR.Text.ToString());
            double HotK = double.Parse(txtHotK.Text.ToString());
            double HotV = double.Parse(txtHViscosity.Text.ToString());

            double HDens = double.Parse(txtHDens.Text.ToString());

            //Initialization of the cold fluid physical properties 
            double CTin = double.Parse(txtCTin.Text.ToString());
            double CTout = double.Parse(txtCTout.Text.ToString());
            double CpCold = double.Parse(txtCpCold.Text.ToString());
            double ColdFr = double.Parse(txtColdFR.Text.ToString());
            double ColdK = double.Parse(txtColdK.Text.ToString());
            double ColdV = double.Parse(txtCViscosity.Text.ToString());

            double CDens = double.Parse(txtColdDens.Text.ToString());

            // Initialization of the assumed properties 

            double Width = double.Parse(txtWidth.Text.ToString());
            double Ua = double.Parse(txtUa.Text.ToString());

            // Initialization of other properties
            double Thick = double.Parse(txtThickness.Text.ToString());
            double ChanS = double.Parse(txtChannelSpac.Text.ToString());
            double CoreD = double.Parse(txtCoreD.Text.ToString());
            double k = double.Parse(txtCondMaterial.Text.ToString());



            //Logic

            double Q = HotFr * CpHot * (HTin - HTout);
            lblQ.Text = Q.ToString();

            double t1 = (HTin - CTout);
            double t2 = (HTout - CTin);

            double lmtd = (t1 - t2) / ((Math.Log(t1 / t2) / (Math.Log(2.71828))));
            lblLMTD.Text = lmtd.ToString();
           // double difU = 1;
           // double tol = 0.01;
            double Uo=1;
            double Area=1;
            double L = 1;
            double Ds = 1;
            double EqD = 1;
            double G = 1;
            double Re = 1;
            double cRe = 1;
            string a="";
            double convCpHot = 1;
            double pR = 1;
            double Hh = 1;
            double EqDC = 1;
            double Gc = 1;
            double ReC = 1;
            string b = "";
            double convCpCold = 1;
            double pRC = 1;
            double HhC = 1;
            double hmHot = 1;
            double pdropHot = 1;
            double hmCold = 1;
            double pdropCold = 1;
            double convQ = 1;
            //possible slate for while loop
        //   while (difU>=tol)
            {

            

            convQ = (Q) * 1000;//Q is first converted to Watt so as to have uniform units with Ua
            Area = convQ / (Ua * lmtd);
           
            
            L = Area / (2 * Width);
           

            Ds = Math.Sqrt((1.28 * L * (2 * ChanS + 2 * Thick) + (CoreD * CoreD)));
            
            //Logic for hot side calculation
            EqD = (2 * ChanS * Width) / (ChanS + Width);
           

            G = HotFr / (ChanS * Width);
            
            
            Re = EqD * G / HotV;
           

           cRe = 20000 * ((Math.Pow(EqD, 0.32)) / (Math.Pow(Ds, 0.32)));
            if (Re > cRe)
            {
                a = "Turbulent";
            }
            else
            {
                a = "Laminar";
            }

            convCpHot = CpHot * 1000; //Converting CpHot to Joules/kgk
            pR = HotV * convCpHot / HotK;
            
            // Hot side heat transfer coefficient
            Hh = (1 + 3.54 * (EqD / Ds)) * 0.023 * convCpHot * G * (Math.Pow(Re, -0.2)) * (Math.Pow(pR, -0.67));
            
            

            // logic for cold side calculation
             EqDC = (2 * ChanS * Width) / (ChanS + Width);
            

            Gc = ColdFr/ (ChanS * Width);
            

             ReC = EqDC * Gc / ColdV;
            lblReynoldC.Text = ReC.ToString();

            double cReC = 20000 * ((Math.Pow(EqDC, 0.32)) / (Math.Pow(Ds, 0.32)));
            if (Re > cReC)
            {
                    b = "Turbulent";
            }
            else
            {
                    b = "Laminar";
            }

            convCpCold = CpCold * 1000; //Converting CpHot to Joules/kgk
             pRC = ColdV * convCpCold / ColdK;
            
            
            HhC = (1 + 3.54 * (EqDC / Ds)) * 0.023 * convCpCold * Gc * (Math.Pow(ReC, -0.2)) * (Math.Pow(pRC, -0.67));
            

             Uo = (1 / ((1 / Hh) + (1 / HhC) + (Thick / k) + (2 * 0.00006666)));

            // logic for pressure drop calculations

            // Hot side
            hmHot = Width / HotFr;
            pdropHot = ((0.0789 * Math.Pow((L / HDens),2) * (HotFr / (Width * ChanS)) * ((1.3 * Math.Pow(HotV, 0.23)) / (ChanS + 0.32)) * (Math.Pow(hmHot, 0.33)) + (1.5 + (16 / L))));
            
            
            //Cold Side
             hmCold = Width / ColdFr;
             pdropCold = ((0.0789 * Math.Pow((L / CDens),2) * (ColdFr / (Width * ChanS)) * ((1.3 * Math.Pow(ColdV, 0.23)) / (ChanS + 0.32)) * (Math.Pow(hmCold, 0.33)) + (1.5 + (16 / L))));

           //  difU = Math.Abs(Uo - Ua);
            //   Ua = Uo;

              }
             
            lblU.Text = Uo.ToString();
            lblA.Text = Area.ToString();
            lblLength.Text = L.ToString();
            lblDs.Text = Ds.ToString();
            lblEDiam.Text = EqD.ToString();
            lblFlux.Text = G.ToString();
            lblReynold.Text = Re.ToString();
            lblFlowtype.Text = a;
            lblPrandtl.Text = pR.ToString();
            lblTransferCo.Text = Hh.ToString();
            lblDiamC.Text = EqDC.ToString();
            lblFluxC.Text = Gc.ToString();
            lblFlowtypeC.Text = b;
            lblPrandtlC.Text = pRC.ToString();
            lblTransfeCoC.Text = HhC.ToString();
            lblPressure.Text = pdropHot.ToString();
            lblPressureC.Text = pdropCold.ToString();


        }
    }
}

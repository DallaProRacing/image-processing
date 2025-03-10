﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Exercicios_1_Proce.Imagens
{
    public partial class RGB : Form
    {
        public RGB()
        {
            InitializeComponent();
        }
        private void ConfigurarEventos()
        {
            // Associando eventos dos TrackBars e NumericUpDowns
            trackBarR.Scroll += TrackBar_Scroll;
            trackBarG.Scroll += TrackBar_Scroll;
            trackBarB.Scroll += TrackBar_Scroll;

            trackBarH.Scroll += TrackBar_Scroll;
            trackBarS.Scroll += TrackBar_Scroll;
            trackBarV.Scroll += TrackBar_Scroll;

            
            trackBarC.Scroll += TrackBar_Scroll;
            trackBarM.Scroll += TrackBar_Scroll;
            trackBarY.Scroll += TrackBar_Scroll;
            trackBarK.Scroll += TrackBar_Scroll;


            nudR.ValueChanged += NumericUpDown_ValueChanged;
            nudG.ValueChanged += NumericUpDown_ValueChanged;
            nudB.ValueChanged += NumericUpDown_ValueChanged;

            nudH.ValueChanged += NumericUpDown_ValueChanged;
            nudS.ValueChanged += NumericUpDown_ValueChanged;
            nudV.ValueChanged += NumericUpDown_ValueChanged;

            nudC.ValueChanged += NumericUpDown_ValueChanged;
            nudM.ValueChanged += NumericUpDown_ValueChanged;
            nudY.ValueChanged += NumericUpDown_ValueChanged;
            nudK.ValueChanged += NumericUpDown_ValueChanged;
        }

        private void TrackBar_Scroll(object sender, EventArgs e)
        {
            // Atualiza os NumericUpDowns conforme os TrackBars se movem
            nudR.Value = trackBarR.Value;
            nudG.Value = trackBarG.Value;
            nudB.Value = trackBarB.Value;

            nudH.Value = trackBarH.Value;
            nudS.Value = trackBarS.Value;
            nudV.Value = trackBarV.Value;

            nudC.Value = trackBarC.Value;
            nudM.Value = trackBarM.Value;
            nudY.Value = trackBarY.Value;
            nudK.Value = trackBarK.Value;

            AtualizarCorPainel();
        }

        private void NumericUpDown_ValueChanged(object sender, EventArgs e)
        {
            // Atualiza os TrackBars conforme os NumericUpDowns mudam
            trackBarR.Value = (int)nudR.Value;
            trackBarG.Value = (int)nudG.Value;
            trackBarB.Value = (int)nudB.Value;

            trackBarH.Value = (int)nudH.Value;
            trackBarS.Value = (int)nudS.Value;
            trackBarV.Value = (int)nudV.Value;

            trackBarC.Value = (int)nudC.Value;
            trackBarM.Value = (int)nudM.Value;
            trackBarY.Value = (int)nudY.Value;
            trackBarK.Value = (int)nudK.Value;

            AtualizarCorPainel();
        }

        private void AtualizarCorPainel()
        {
            int R = trackBarR.Value;
            int G = trackBarG.Value;
            int B = trackBarB.Value;

            int H = trackBarH.Value;
            int S = trackBarS.Value;
            int V = trackBarV.Value;

            int C = trackBarC.Value;
            int M = trackBarM.Value;
            int Y = trackBarY.Value;
            int K = trackBarK.Value;

            // Atualiza o painel RGB normalmente
            panelCorRGB.BackColor = Color.FromArgb(R, G, B);

            // Converte HSV para RGB e aplica no painel
            (int rHSV, int gHSV, int bHSV) = HSVtoRGB(H, S, V);
            panelHSV.BackColor = Color.FromArgb(rHSV, gHSV, bHSV);

            (int rCMYK, int gCMYK, int bCMYK) = CMYKtoRGB(C, M, Y, K);
            panelCMYK.BackColor = Color.FromArgb(rCMYK, gCMYK, bCMYK);
        }

        private (double, double, double, double) RGBtoCMYK(int r, int g, int b)
        {
            double c = 1 - (r / 255.0);
            double m = 1 - (g / 255.0);
            double y = 1 - (b / 255.0);

            double k = Math.Min(c, Math.Min(m, y));

            if (k == 1)
                return (0, 0, 0, 1);

            c = (c - k) / (1 - k);
            m = (m - k) / (1 - k);
            y = (y - k) / (1 - k);

            return (c, m, y, k);
        }

        private (double, double, double) RGBtoHSV(int r, int g, int b)
        {
            double rNorm = r / 255.0, gNorm = g / 255.0, bNorm = b / 255.0;
            double max = Math.Max(rNorm, Math.Max(gNorm, bNorm));
            double min = Math.Min(rNorm, Math.Min(gNorm, bNorm));
            double delta = max - min;

            double h = 0, s = (max == 0) ? 0 : delta / max, v = max;

            if (delta != 0)
            {
                if (max == rNorm)
                    h = 60 * (((gNorm - bNorm) / delta) % 6);
                else if (max == gNorm)
                    h = 60 * (((bNorm - rNorm) / delta) + 2);
                else if (max == bNorm)
                    h = 60 * (((rNorm - gNorm) / delta) + 4);
            }

            if (h < 0)
                h += 360;

            return (h, s*100, v*100);
        }
        private (int, int, int) HSVtoRGB(double h, double s, double v)
        {
            // Converter valores para a escala correta
            double hDegrees = (h / 255.0) * 360;  // De 1-255 para 0-360 graus
            double sPercent = (s / 255.0) * 100;  // De 1-255 para 0-100%
            double vPercent = (v / 255.0) * 100;  // De 1-255 para 0-100%

            // Normalizar valores para a fórmula
            double sNorm = sPercent / 100;
            double vNorm = vPercent / 100;

            double c = vNorm * sNorm;
            double x = c * (1 - Math.Abs((hDegrees / 60.0) % 2 - 1));
            double m = vNorm - c;

            double r, g, b;
            if (hDegrees >= 0 && hDegrees < 60) { r = c; g = x; b = 0; }
            else if (hDegrees >= 60 && hDegrees < 120) { r = x; g = c; b = 0; }
            else if (hDegrees >= 120 && hDegrees < 180) { r = 0; g = c; b = x; }
            else if (hDegrees >= 180 && hDegrees < 240) { r = 0; g = x; b = c; }
            else if (hDegrees >= 240 && hDegrees < 300) { r = x; g = 0; b = c; }
            else { r = c; g = 0; b = x; }

            return (
                (int)Math.Round((r + m) * 255),
                (int)Math.Round((g + m) * 255),
                (int)Math.Round((b + m) * 255)
            );
        }




        private (int, int, int) CMYKtoRGB(double C, double M, double Y, double K)
        {
            // Converte os valores CMYK (0-100) para o intervalo 0-1
            double c = C / 100.0;
            double m = M / 100.0;
            double y = Y / 100.0;
            double k = K / 100.0;

            // Caso em que K é 1 (preto completo), todos os valores RGB serão 0
            if (k == 1)
            {
                return (0, 0, 0);
            }

            // Calcula os valores RGB usando a fórmula ajustada
            int R = (int)((1 - c) * (1 - k) * 255);
            int G = (int)((1 - m) * (1 - k) * 255);
            int B = (int)((1 - y) * (1 - k) * 255);

            // Garante que os valores RGB estejam entre 0 e 255
            R = Math.Max(0, Math.Min(255, R));
            G = Math.Max(0, Math.Min(255, G));
            B = Math.Max(0, Math.Min(255, B));

            return (R, G, B);
        }



        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void btnConvertBRG_Click(object sender, EventArgs e)
        {
            int r = (int)nudR.Value;
            int g = (int)nudG.Value;
            int b = (int)nudB.Value;

            // RGB para CMYK
            var (c, m, y, k) = RGBtoCMYK(r, g, b);
            textBoxConvertCMYK.Text = $"C: {c:F2}, M: {m:F2}, Y: {y:F2}, K: {k:F2}";

            // RGB para HSV
            var (h, s, v) = RGBtoHSV(r, g, b);
            textBoxConvertHSV.Text = $"H: {h:F0}°, S: {s:F2}%, V: {v:F2}%";

            // Escala de Cinza
            int gray = (int)(0.3 * r + 0.59 * g + 0.11 * b);
            textBoxConvertEscala.Text = gray.ToString();

            // RGB Normalizado
            textBoxConvertNormal.Text = $"R: {r / 255.0 * 100 :F2}, G: {g / 255.0 * 100:F2}, B: {b / 255.0 * 100:F2}";
        }

        private void btnConvertHSV_RGB_Click(object sender, EventArgs e)
        {
            double h = (double)nudH.Value;
            double s = (double)nudS.Value;
            double v = (double)nudV.Value;

            var (r, g, b) = HSVtoRGB(h, s, v);

            textBoxConvertR.Text = r.ToString();
            textBoxConvertG.Text = g.ToString();
            textBoxConvertB.Text = b.ToString();
        }
        
        private void btnConvertCMYK_RGB_Click(object sender, EventArgs e)
        {
            // CMYK valores são passados em porcentagem (0-100)
            double c = (double)nudC.Value;
            double m = (double)nudM.Value;
            double y = (double)nudY.Value;
            double k = (double)nudK.Value;

            // Converte de CMYK para RGB
            var (r, g, b) = CMYKtoRGB(c, m, y, k);

            // Atualiza os valores convertidos
            textBoxConvertCMYK_R.Text = r.ToString();
            textBoxConvertCMYK_G.Text = g.ToString();
            textBoxConvertCMYK_B.Text = b.ToString();
        }
    }
}

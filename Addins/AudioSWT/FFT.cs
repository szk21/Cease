using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AudioSWT
{
    class FFT
    {
        const double PI = 3.1415926535897932384626433832795028841;

        //FFT transformation
        // 入口参数： 
        // l: l = 0, 傅立叶变换; l = 1, 逆傅立叶变换
        // il: il = 0,不计算傅立叶变换或逆变换模和幅角；il = 1,计算模和幅角
        // n: 输入的点数，为偶数，一般为32，64，128，...,1024等
        // k: 满足n=2^k(k>0),实质上k是n个采样数据可以分解为偶次幂和奇次幂的次数
        // pr[]: l=0时，存放N点采样数据的实部
        // l=1时, 存放傅立叶变换的N个实部
        // pi[]: l=0时，存放N点采样数据的虚部 
        // l=1时, 存放傅立叶变换的N个虚部
        //
        // 出口参数：
        // fr[]: l=0, 返回傅立叶变换的实部
        // l=1, 返回逆傅立叶变换的实部
        // fi[]: l=0, 返回傅立叶变换的虚部
        // l=1, 返回逆傅立叶变换的虚部
        // pr[]: il = 1,l = 0 时，返回傅立叶变换的模
        // il = 1,l = 1 时，返回逆傅立叶变换的模
        // pi[]: il = 1,l = 0 时，返回傅立叶变换的辐角
        // il = 1,l = 1 时，返回逆傅立叶变换的辐角
        public static void FFT_Calc(double[] pr, double[] pi, int n, int k, ref double[] fr, ref double[] fi, int l, int il)
        {
            int it, m, iss, i, j, nv, l0;
            double p, q, s, vr, vi, poddr, poddi;
            //排序
            for (it = 0; it <= n - 1; it++)
            {
                m = it; iss = 0;
                for (i = 0; i <= k - 1; i++)
                {
                    j = m / 2; iss = 2 * iss + (m - 2 * j); m = j;
                    fr[it] = pr[iss]; fi[it] = pi[iss];
                }
            }

            //蝶形运算
            pr[0] = 1.0; pi[0] = 0.0;
            p = 6.283185306 / (1.0 * n);
            pr[1] = Math.Cos(p); pi[1] = -Math.Sin(p);
            if (l != 0) pi[1] = -pi[1];
            for (i = 2; i <= n - 1; i++)
            {
                p = pr[i - 1] * pr[1]; q = pi[i - 1] * pi[1];
                s = (pr[i - 1] + pi[i - 1]) * (pr[1] + pi[1]);
                pr[i] = p - q; pi[i] = s - p - q;
            }
            for (it = 0; it <= n - 2; it = it + 2)
            {
                vr = fr[it]; vi = fi[it];
                fr[it] = vr + fr[it + 1]; fi[it] = vi + fi[it + 1];
                fr[it + 1] = vr - fr[it + 1]; fi[it + 1] = vi - fi[it + 1];
            }
            m = n / 2; nv = 2;
            for (l0 = k - 2; l0 >= 0; l0--)
            {
                m = m / 2; nv = 2 * nv;
                for (it = 0; it <= (m - 1) * nv; it = it + nv)
                    for (j = 0; j <= (nv / 2) - 1; j++)
                    {
                        p = pr[m * j] * fr[it + j + nv / 2];
                        q = pi[m * j] * fi[it + j + nv / 2];
                        s = pr[m * j] + pi[m * j];
                        s = s * (fr[it + j + nv / 2] + fi[it + j + nv / 2]);
                        poddr = p - q; poddi = s - p - q;
                        fr[it + j + nv / 2] = fr[it + j] - poddr;
                        fi[it + j + nv / 2] = fi[it + j] - poddi;
                        fr[it + j] = fr[it + j] + poddr;
                        fi[it + j] = fi[it + j] + poddi;
                    }
            }
            if (l != 0)
                for (i = 0; i <= n - 1; i++)
                {
                    fr[i] = fr[i] / (1.0 * n);
                    fi[i] = fi[i] / (1.0 * n);
                }
            if (il != 0)
                for (i = 0; i <= n - 1; i++)
                {
                    pr[i] = Math.Sqrt(fr[i] * fr[i] + fi[i] * fi[i]);
                    pr[i] = (pr[i] / (n / 2)); //各次谐波幅值，其中pr[1]为基波幅值
                    if (Math.Abs(fr[i]) < 0.000001 * Math.Abs(fi[i]))
                    //fabs()是取绝对值函数,浮点型的0 在内存中并不是严格等于0,可以认为当一个浮点数离原点足够近时,也就是f>0.00001 && f<-0.00001,认为f是0
                    {
                        if ((fi[i] * fr[i]) > 0) pi[i] = 90.0;
                        else pi[i] = -90.0;
                    }
                    else
                        pi[i] = Math.Atan(fi[i] / fr[i]) * 360.0 / 6.283185306;
                }
        }

        /// <summary>
        /// 计算频谱
        /// </summary>
        /// <param name="NumSamples"></param>
        /// <param name="pReal"></param>
        /// <param name="pImag"></param>
        /// <param name="pAmpl"></param>
        public static void FFT_Spectrum(int NumSamples, Double[] pReal, Double[] pImag, ref Double[] pAmpl)
        {
            if (pReal == null || pImag == null || pAmpl == null)
            {
                // error
                throw new ArgumentNullException("pReal,pImag,pAmpl");
            }
            if (pReal.Length < NumSamples || pImag.Length < NumSamples || pAmpl.Length < NumSamples)
            {
                // error
                throw new ArgumentException("Invalid Array argument detected");
            }

            // Calculate amplitude values in the buffer provided
            for (int i = 0; i < NumSamples; i++)
            {
                pAmpl[i] = Math.Sqrt(pReal[i] * pReal[i] + pImag[i] * pImag[i]);
            }
        }

        /// <summary>
        /// 汉宁窗
        /// </summary>
        /// <param name="N">点数</param>
        /// <param name="w">返回:窗口曲线</param>
        public static void hannWin(int N, ref double[] w)
        {

            for (int n = 0; n < N; n++)
            {
                w[n] = 0.5 * (1 - Math.Cos(2 * PI * (double)n / (N - 1)));
            }

            return;
        }
    }
}

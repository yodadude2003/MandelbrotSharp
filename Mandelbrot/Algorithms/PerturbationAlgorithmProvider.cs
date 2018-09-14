﻿using Mandelbrot.Imaging;
using Mandelbrot.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mandelbrot.Algorithms
{
    class PerturbationAlgorithmProvider<T> : IAlgorithmProvider<T>
    {
        private IGenericMath<T> TMath;
        private List<GenericComplex<T>> iterList;

        private T Zero;
        private T OneHalf;
        private T TwoPow8;
        private T NegTwoPow8;
        private T TwoPow10;
        private T NegTwoPow10;

        private T center_real;
        private T center_imag;

        private int MaxIterations;

        // Perturbation Theory Algorithm, 
        // produces a list of iteration values used to compute the surrounding points
        public void Init(IGenericMath<T> TMath, T offsetX, T offsetY, int maxIterations)
        {
            this.TMath = TMath;
            MaxIterations = maxIterations;

            Zero = TMath.fromInt32(0);
            OneHalf = TMath.fromDouble(0.5);
            TwoPow8 = TMath.fromInt32(256);
            NegTwoPow8 = TMath.fromInt32(-256);
            TwoPow10 = TMath.fromInt32(1024);
            NegTwoPow10 = TMath.fromInt32(-1024);

            center_real = offsetX;
            center_imag = offsetY;

            GetIterationList();
        }

        public void GetIterationList()
        {
            T xn_r = center_real;
            T xn_i = center_imag;

            iterList = new List<GenericComplex<T>>();

            for (int i = 0; i < MaxIterations; i++)
            {
                // pre multiply by two
                T real = TMath.Add(xn_r, xn_r);
                T imag = TMath.Add(xn_i, xn_i);

                T xn_r2 = TMath.Multiply(xn_r, xn_r);
                T xn_i2 = TMath.Multiply(xn_i, xn_i);

                GenericComplex<T> c = new GenericComplex<T>(real, imag);

                iterList.Add(c);

                // make sure our numbers don't get too big

                // real > 1024 || imag > 1024 || real < -1024 || imag < -1024
                if (TMath.GreaterThan(real, TwoPow10) || TMath.GreaterThan(imag, TwoPow10) ||
                    TMath.LessThan(real, NegTwoPow10) || TMath.LessThan(imag, NegTwoPow10))
                    break;

                // calculate next iteration, remember real = 2 * xn_r

                // xn_r = xn_r^2 - xn_i^2 + center_r
                xn_r = TMath.Add(TMath.Subtract(xn_r2, xn_i2), center_real);
                // xn_i = re * xn_i + center_i
                xn_i = TMath.Add(TMath.Multiply(real, xn_i), center_imag);
            }
            return;
        }

        // Non-Traditional Mandelbrot algorithm, 
        // Iterates a point over its neighbors to approximate an iteration count.
        public PixelData<T> Run(T x0, T y0)
        {
            ComplexMath<T> CMath = new ComplexMath<T>(TMath);

            // Get max iterations.  
            int maxIterations = iterList.Count;

            // Initialize our iteration count.
            int iterCount = 0;

            // Initialize some variables...
            GenericComplex<T> zn;

            GenericComplex<T> d0 = new GenericComplex<T>(x0, y0);

            GenericComplex<T> dn = d0;

            T znMagn = Zero;

            // Mandelbrot algorithm
            while (TMath.LessThan(znMagn, TwoPow8) && iterCount < maxIterations)
            {

                // dn *= iter_list[iter] + dn
                dn = CMath.Multiply(dn, CMath.Add(iterList[iterCount], dn));

                // dn += d0
                dn = CMath.Add(dn, d0);

                // zn = x[iter] * 0.5 + dn
                zn = CMath.Add(CMath.Multiply(iterList[iterCount], OneHalf), dn);

                znMagn = CMath.MagnitudeSquared(zn);

                iterCount++;
            }

            return new PixelData<T>(znMagn, iterCount, iterCount < maxIterations);
        }
    }
}

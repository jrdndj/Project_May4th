/**
 * FittsStudy
 *
 *		Jacob O. Wobbrock, Ph.D.
 * 		The Information School
 *		University of Washington
 *		Mary Gates Hall, Box 352840
 *		Seattle, WA 98195-2840
 *		wobbrock@uw.edu
 *		
 * This software is distributed under the "New BSD License" agreement:
 * 
 * Copyright (c) 2007-2011, Jacob O. Wobbrock. All rights reserved.
 *
 * Redistribution and use in source and binary forms, with or without
 * modification, are permitted provided that the following conditions are met:
 *    * Redistributions of source code must retain the above copyright
 *      notice, this list of conditions and the following disclaimer.
 *    * Redistributions in binary form must reproduce the above copyright
 *      notice, this list of conditions and the following disclaimer in the
 *      documentation and/or other materials provided with the distribution.
 *    * Neither the name of the University of Washington nor the names of its 
 *      contributors may be used to endorse or promote products derived from 
 *      this software without specific prior written permission.
 *
 * THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS
 * IS" AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO,
 * THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR
 * PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL Jacob O. Wobbrock
 * BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR 
 * CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE 
 * GOODS OR SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) 
 * HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT
 * LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY
 * OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF
 * SUCH DAMAGE.
**/
using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using WobbrockLib.Types;

namespace FittsStudy
{
    /// <summary>
    /// This structure holds information concerning a fitted Fitts' law model for
    /// movement time data. The model is MTe = a + b * log2(Ae/We + 1).
    /// </summary>
    public struct Model
    {
        /// <summary>
        /// A static initializer that can be used for null values.
        /// </summary>
        public static readonly Model Empty;

        /// <summary>
        /// The number of points over which the regression was conducted.
        /// </summary>
        public int N;

        /// <summary>
        /// The (IDe, MTe) points over which the regression was performed.
        /// </summary>
        public List<PointR> FittsPts_1d;
        public List<PointR> FittsPts_2d;

        /// <summary>
        /// The (predicted error rate, observed error rate) points for the error model of pointing.
        /// </summary>
        public List<PointR> ErrorPts_1d;
        public List<PointR> ErrorPts_2d;

        /// <summary>
        /// The grand mean of the effective movement time for all conditions in milliseconds.
        /// </summary>
        public double MTe;

        /// <summary>
        /// The throughput measure in bits/s. This is a measure that combines speed and
        /// accuracy. It is calculated as IDe/MT * 1000, which is the mean of throughputs from
        /// each condition.
        /// </summary>
        public double Fitts_TP_avg_1d;
        public double Fitts_TP_avg_2d;

        /// <summary>
        /// The throughput measure in bits/s. This is a measure that combines speed and
        /// accuracy. It is calculated as 1/b * 1000, which is the reciprocal of the regression
        /// slope. It is still debated whether this is better than the method above or not.
        /// </summary>
        public double Fitts_TP_inv_1d;
        public double Fitts_TP_inv_2d;

        /// <summary>
        /// The 'a' regression coefficient for this model. This is the intercept for the
        /// regression line, measured in milliseconds. Equation is MT = a + b*IDe.
        /// </summary>
        public double Fitts_a_1d;
        public double Fitts_a_2d;

        /// <summary>
        /// The 'b' regression coefficient for this model. This is the slope of the
        /// regression line, measured in ms/bit. Equation is MT = a + b*IDe.
        /// </summary>
        public double Fitts_b_1d;
        public double Fitts_b_2d;

        /// <summary>
        /// The Pearson coefficient of correlation for the regression fitting.
        /// </summary>
        public double Fitts_r_1d;
        public double Fitts_r_2d;

        /// <summary>
        /// The grand mean of the error rate for all conditions. Outliers are included, so
        /// this value reflects all errors, even if they are outliers.
        /// </summary>
        public double ErrorPct;

        /// <summary>
        /// The error model for pointing slope for the (predicted error rate, observed error rate)
        /// points. A "perfect model" would have a unity slope for this graph. Equation is Observed = m*Predicted + b.
        /// </summary>
        public double Error_m_1d;
        public double Error_m_2d;

        /// <summary>
        /// The error model for pointing intercept for the (predicted error rate, observed error rate)
        /// points. A "perfect model" would have a zero intercept for this graph. Equation is Observed = m*Predicted + b.
        /// </summary>
        public double Error_b_1d;
        public double Error_b_2d;

        /// <summary>
        /// The Pearson coefficient of correlation for the regression fitting for the error model of pointing.
        /// </summary>
        public double Error_r_1d;
        public double Error_r_2d;

        /// <summary>
        /// Rounds the model terms to a significant number of <i>digits</i>.
        /// </summary>
        /// <param name="digits"></param>
        public void RoundTerms(int digits)
        {
            this.MTe = Math.Round(MTe, digits);
            
            this.Fitts_TP_avg_1d = Math.Round(Fitts_TP_avg_1d, digits);
            this.Fitts_TP_avg_2d = Math.Round(Fitts_TP_avg_2d, digits);

            this.Fitts_TP_inv_1d = Math.Round(Fitts_TP_inv_1d, digits);
            this.Fitts_TP_inv_2d = Math.Round(Fitts_TP_inv_2d, digits);

            this.Fitts_a_1d = Math.Round(Fitts_a_1d, digits);
            this.Fitts_a_2d = Math.Round(Fitts_a_2d, digits);

            this.Fitts_b_1d = Math.Round(Fitts_b_1d, digits);
            this.Fitts_b_2d = Math.Round(Fitts_b_2d, digits);

            this.Fitts_r_1d = Math.Round(Fitts_r_1d, digits);
            this.Fitts_r_2d = Math.Round(Fitts_r_2d, digits);

            this.ErrorPct = Math.Round(ErrorPct, digits);

            this.Error_m_1d = Math.Round(Error_m_1d, digits);
            this.Error_m_2d = Math.Round(Error_m_2d, digits);

            this.Error_b_1d = Math.Round(Error_b_1d, digits);
            this.Error_b_2d = Math.Round(Error_b_2d, digits);

            this.Error_r_1d = Math.Round(Error_r_1d, digits);
            this.Error_r_2d = Math.Round(Error_r_2d, digits);
        }
    }
}

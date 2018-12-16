using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace AreaCalculator
{
    interface IFigure
    {
        bool Check(double[] vals);

        double AreaOf(double[] vals);
    }
    public class Calculator
    {
        public enum Figure {Undefined, Circle, Triangle }

        public double AreaOf(double[] vals,Figure figType = Figure.Undefined)
        {
            vals = vals.OrderByDescending(x => x).ToArray();
            double result = 0;
            try
            {
                if (figType == Figure.Undefined)
                {
                    figType = DetermineFigure(vals);
                }
                try
                {
                    Assembly assembly = Assembly.GetExecutingAssembly();
                    Type type = assembly.GetTypes()
                        .First(t => t.Name == figType.ToString());
                    IFigure figure;
                    figure = (IFigure)Activator.CreateInstance(type);
                    result = DoWork(vals, figure);
                }
                catch(Exception e)
                {
                    throw (new Exception("Loading a generic type caused an error! ",e));
                }
            }
            catch (Exception)
            {
                //maybe some logging is in order
                throw;
            }
            return result;
        }
        double DoWork <T>(double[] vals, T t) where T:IFigure
        {
            double result = 0;
            if (t.Check(vals))
               result = t.AreaOf(vals);
            return result;
        }

        private Figure DetermineFigure(double[] vals)
        {
            Figure result = Figure.Undefined;
            switch (vals.Length)
            { 
                case 1:
                    {
                        result = Figure.Circle;
                        break;
                    }
                case 3:
                    {
                        result = Figure.Triangle;
                        break;
                    }
            }
            return result;

        }

        private class Circle : IFigure
        {
            public double AreaOf(double[] vals)
            {
                return Math.PI * vals[0] * vals[0];
            }

            public bool Check(double[] vals)
            {
                bool result = false;
                try
                {
                    if (vals[0] >= 0)
                    {
                        result = true;
                    }
                    else
                        throw( new Exception());
                }
                catch (Exception e)
                {
                    throw new ArgumentException("Entered argument collection containted errors", e);
                }
                return result;
            }
        }

        private class Triangle : IFigure
        {
            public double AreaOf(double[] vals)
            {
                double result = 0;
                if (isRightTriangle(vals))
                    result = vals[1] * vals[2]/2;
                else
                {
                    double p = vals.Sum() / 2;
                    result = Math.Sqrt(p * (p - vals[0]) * (p - vals[1]) * (p - vals[2]));
                }
                return result;
            }
            private bool isRightTriangle(double[] vals)
            {
                bool result = false;
                double max = vals[0];
                if (Math.Pow(max, 2) == Math.Pow(vals[1] , 2)+ Math.Pow(vals[2], 2))
                    result = true;
                return result;
            }
            public bool Check(double[] vals)
            {
                bool result = false;
                try
                {
                    if (vals[0] >= 0 && (vals.Sum()>vals.Max()*2))
                    {
                        result = true;
                    }
                    else throw new Exception();
                }
                catch (Exception e)
                {
                    throw new ArgumentException("Entered argument string containted errors", e);
                }
                return result;
            }
        }
    }

}

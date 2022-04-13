namespace BusbarReader.RvtAddin
{
    public struct Vector3d
    {
        public double x;

        public double y;

        public double z;

        public static readonly Vector3d Zero = new Vector3d(0.0, 0.0, 0.0);

        public static readonly Vector3d One = new Vector3d(1.0, 1.0, 1.0);

        public static readonly Vector3d AxisX = new Vector3d(1.0, 0.0, 0.0);

        public static readonly Vector3d AxisY = new Vector3d(0.0, 1.0, 0.0);

        public static readonly Vector3d AxisZ = new Vector3d(0.0, 0.0, 1.0);

        public static readonly Vector3d MaxValue = new Vector3d(double.MaxValue, double.MaxValue, double.MaxValue);

        public static readonly Vector3d MinValue = new Vector3d(double.MinValue, double.MinValue, double.MinValue);

        public double this[int key]
        {
            get
            {
                switch (key)
                {
                    default:
                        return z;
                    case 1:
                        return y;
                    case 0:
                        return x;
                }
            }
            set
            {
                switch (key)
                {
                    case 0:
                        x = value;
                        break;
                    case 1:
                        y = value;
                        break;
                    default:
                        z = value;
                        break;
                }
            }
        }

        public double LengthSquared => x * x + y * y + z * z;

        public double Length => Math.Sqrt(LengthSquared);

        public double LengthL1 => Math.Abs(x) + Math.Abs(y) + Math.Abs(z);

        public double Max => Math.Max(x, Math.Max(y, z));

        public double Min => Math.Min(x, Math.Min(y, z));

        public double MaxAbs => Math.Max(Math.Abs(x), Math.Max(Math.Abs(y), Math.Abs(z)));

        public double MinAbs => Math.Min(Math.Abs(x), Math.Min(Math.Abs(y), Math.Abs(z)));

        public Vector3d Abs => new Vector3d(Math.Abs(x), Math.Abs(y), Math.Abs(z));

        public Vector3d Normalized
        {
            get
            {
                double length = Length;
                if (length > 2.2204460492503131E-16)
                {
                    double num = 1.0 / length;
                    return new Vector3d(x * num, y * num, z * num);
                }

                return Zero;
            }
        }

        public bool IsNormalized => Math.Abs(x * x + y * y + z * z - 1.0) < 1E-08;

        public bool IsFinite
        {
            get
            {
                double d = x + y + z;
                if (!double.IsNaN(d))
                {
                    return !double.IsInfinity(d);
                }

                return false;
            }
        }

        public Vector3d(double f)
        {
            x = (y = (z = f));
        }

        public Vector3d(double x, double y, double z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }

        public Vector3d(double[] v2)
        {
            x = v2[0];
            y = v2[1];
            z = v2[2];
        }

        public Vector3d(Vector3d copy)
        {
            x = copy.x;
            y = copy.y;
            z = copy.z;
        }

        public double Normalize(double epsilon = 2.2204460492503131E-16)
        {
            double num = Length;
            if (num > epsilon)
            {
                double num2 = 1.0 / num;
                x *= num2;
                y *= num2;
                z *= num2;
            }
            else
            {
                num = 0.0;
                x = (y = (z = 0.0));
            }

            return num;
        }

        public void Round(int nDecimals)
        {
            x = Math.Round(x, nDecimals);
            y = Math.Round(y, nDecimals);
            z = Math.Round(z, nDecimals);
        }

        public double Dot(Vector3d v2)
        {
            return x * v2.x + y * v2.y + z * v2.z;
        }

        public double Dot(ref Vector3d v2)
        {
            return x * v2.x + y * v2.y + z * v2.z;
        }

        public static double Dot(Vector3d v1, Vector3d v2)
        {
            return v1.Dot(ref v2);
        }

        public Vector3d Cross(Vector3d v2)
        {
            return new Vector3d(y * v2.z - z * v2.y, z * v2.x - x * v2.z, x * v2.y - y * v2.x);
        }

        public Vector3d Cross(ref Vector3d v2)
        {
            return new Vector3d(y * v2.z - z * v2.y, z * v2.x - x * v2.z, x * v2.y - y * v2.x);
        }

        public static Vector3d Cross(Vector3d v1, Vector3d v2)
        {
            return v1.Cross(ref v2);
        }

        public Vector3d UnitCross(ref Vector3d v2)
        {
            Vector3d result = new Vector3d(y * v2.z - z * v2.y, z * v2.x - x * v2.z, x * v2.y - y * v2.x);
            result.Normalize();
            return result;
        }

        public Vector3d UnitCross(Vector3d v2)
        {
            return UnitCross(ref v2);
        }

        public static double AngleR(Vector3d vector1, Vector3d vector2)
        {
            vector1.Normalize();
            vector2.Normalize();
            double num = Dot(vector1, vector2);
            double radians = (!(num < 0.0)) ? (2.0 * Math.Asin((vector1 - vector2).Length / 2.0)) : (Math.PI - 2.0 * Math.Asin((-vector1 - vector2).Length / 2.0));
            return radians;
        }

        public static bool IsPara(Vector3d vector1, Vector3d vector2, double eplison)
        {
            var angel = Math.Abs(AngleR(vector1, vector2));

            return angel< eplison || Math.Abs(angel - Math.PI) < eplison;
        }

        public double DistanceSquared(Vector3d v2)
        {
            double num = v2.x - x;
            double num2 = v2.y - y;
            double num3 = v2.z - z;
            return num * num + num2 * num2 + num3 * num3;
        }

        public double DistanceSquared(ref Vector3d v2)
        {
            double num = v2.x - x;
            double num2 = v2.y - y;
            double num3 = v2.z - z;
            return num * num + num2 * num2 + num3 * num3;
        }

        public double Distance(Vector3d v2)
        {
            double num = v2.x - x;
            double num2 = v2.y - y;
            double num3 = v2.z - z;
            return Math.Sqrt(num * num + num2 * num2 + num3 * num3);
        }

        public double Distance(ref Vector3d v2)
        {
            double num = v2.x - x;
            double num2 = v2.y - y;
            double num3 = v2.z - z;
            return Math.Sqrt(num * num + num2 * num2 + num3 * num3);
        }

        public void Set(Vector3d o)
        {
            x = o.x;
            y = o.y;
            z = o.z;
        }

        public void Set(double fX, double fY, double fZ)
        {
            x = fX;
            y = fY;
            z = fZ;
        }

        public void Add(Vector3d o)
        {
            x += o.x;
            y += o.y;
            z += o.z;
        }

        public void Subtract(Vector3d o)
        {
            x -= o.x;
            y -= o.y;
            z -= o.z;
        }

        public static Vector3d operator -(Vector3d v)
        {
            return new Vector3d(0.0 - v.x, 0.0 - v.y, 0.0 - v.z);
        }

        public static Vector3d operator *(double f, Vector3d v)
        {
            return new Vector3d(f * v.x, f * v.y, f * v.z);
        }

        public static Vector3d operator *(Vector3d v, double f)
        {
            return new Vector3d(f * v.x, f * v.y, f * v.z);
        }

        public static Vector3d operator /(Vector3d v, double f)
        {
            return new Vector3d(v.x / f, v.y / f, v.z / f);
        }

        public static Vector3d operator /(double f, Vector3d v)
        {
            return new Vector3d(f / v.x, f / v.y, f / v.z);
        }

        public static Vector3d operator *(Vector3d a, Vector3d b)
        {
            return new Vector3d(a.x * b.x, a.y * b.y, a.z * b.z);
        }

        public static Vector3d operator /(Vector3d a, Vector3d b)
        {
            return new Vector3d(a.x / b.x, a.y / b.y, a.z / b.z);
        }

        public static Vector3d operator +(Vector3d v0, Vector3d v1)
        {
            return new Vector3d(v0.x + v1.x, v0.y + v1.y, v0.z + v1.z);
        }

        public static Vector3d operator +(Vector3d v0, double f)
        {
            return new Vector3d(v0.x + f, v0.y + f, v0.z + f);
        }

        public static Vector3d operator -(Vector3d v0, Vector3d v1)
        {
            return new Vector3d(v0.x - v1.x, v0.y - v1.y, v0.z - v1.z);
        }

        public static Vector3d operator -(Vector3d v0, double f)
        {
            return new Vector3d(v0.x - f, v0.y - f, v0.z - f);
        }

        public bool EpsilonEqual(Vector3d v2, double epsilon)
        {
            if (Math.Abs(x - v2.x) <= epsilon && Math.Abs(y - v2.y) <= epsilon)
            {
                return Math.Abs(z - v2.z) <= epsilon;
            }

            return false;
        }

        public static Vector3d Lerp(Vector3d a, Vector3d b, double t)
        {
            double num = 1.0 - t;
            return new Vector3d(num * a.x + t * b.x, num * a.y + t * b.y, num * a.z + t * b.z);
        }

        public static Vector3d Lerp(ref Vector3d a, ref Vector3d b, double t)
        {
            double num = 1.0 - t;
            return new Vector3d(num * a.x + t * b.x, num * a.y + t * b.y, num * a.z + t * b.z);
        }

        public override string ToString()
        {
            return $"{x:F8} {y:F8} {z:F8}";
        }

        public string ToString(string fmt)
        {
            return $"{x.ToString(fmt)} {y.ToString(fmt)} {z.ToString(fmt)}";
        }

        public static double Orthonormalize(ref Vector3d u, ref Vector3d v, ref Vector3d w)
        {
            double num = u.Normalize();
            double f = u.Dot(v);
            v -= f * u;
            double num2 = v.Normalize();
            if (num2 < num)
            {
                num = num2;
            }

            double f2 = v.Dot(w);
            f = u.Dot(w);
            w -= f * u + f2 * v;
            num2 = w.Normalize();
            if (num2 < num)
            {
                num = num2;
            }

            return num;
        }

        public static double ComputeOrthogonalComplement(int numInputs, Vector3d v0, ref Vector3d v1, ref Vector3d v2)
        {
            if (numInputs == 1)
            {
                if (Math.Abs(v0[0]) > Math.Abs(v0[1]))
                {
                    v1 = new Vector3d(0.0 - v0[2], 0.0, v0[0]);
                }
                else
                {
                    v1 = new Vector3d(0.0, v0[2], 0.0 - v0[1]);
                }

                numInputs = 2;
            }

            if (numInputs == 2)
            {
                v2 = Cross(v0, v1);
                return Orthonormalize(ref v0, ref v1, ref v2);
            }

            return 0.0;
        }

        public static void MakePerpVectors(ref Vector3d n, out Vector3d b1, out Vector3d b2)
        {
            if (n.z < 0.0)
            {
                double num = 1.0 / (1.0 - n.z);
                double num2 = n.x * n.y * num;
                b1.x = 1.0 - n.x * n.x * num;
                b1.y = 0.0 - num2;
                b1.z = n.x;
                b2.x = num2;
                b2.y = n.y * n.y * num - 1.0;
                b2.z = 0.0 - n.y;
            }
            else
            {
                double num3 = 1.0 / (1.0 + n.z);
                double num4 = (0.0 - n.x) * n.y * num3;
                b1.x = 1.0 - n.x * n.x * num3;
                b1.y = num4;
                b1.z = 0.0 - n.x;
                b2.x = num4;
                b2.y = 1.0 - n.y * n.y * num3;
                b2.z = 0.0 - n.y;
            }
        }
    }
}
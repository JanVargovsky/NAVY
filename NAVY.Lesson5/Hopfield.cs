using MathNet.Numerics.LinearAlgebra;
using System;

namespace NAVY.Lesson5
{
    class Hopfield
    {
        readonly int n;
        Matrix<float> w;

        public Hopfield(int n)
        {
            this.n = n;

        }

        Vector<float> Normalize(Vector<float> u)
        {
            var ux = Vector<float>.Build.Dense(u.Count);
            for (int i = 0; i < u.Count; i++)
                ux[i] = u[i] == 1 ? 1 : -1;
            return ux;
        }

        public void Train(params Vector<float>[] us)
        {
            if (us.Length == 0)
                return;

            int size = us[0].Count;
            this.w = Matrix<float>.Build.Sparse(size, size);

            foreach (var u in us)
            {
                var ux = Normalize(u);
                var w = ux.ToColumnMatrix() * ux.ToRowMatrix();
                var i = Matrix<float>.Build.SparseIdentity(u.Count);
                w -= i;

                this.w += w;
            }
        }

        public Vector<float> Recognise(Vector<float> u)
        {
            var ux = Normalize(u);
            Vector<float> s = Vector<float>.Build.SameAs(ux);

            var x = ux;
            for (int i = 0; i < u.Count; i++)
            {
                var c = w.Column(i);
                var t = x * c;
                var f = Math.Sign(t);
                s[i] = f;
            }

            return s;
        }
    }
}

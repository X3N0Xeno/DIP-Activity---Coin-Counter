using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;

namespace DIP_Activity___Coin_Counter
{
    class CoinResult
    {
        public Bitmap Image;
        public int[] Counts = new int[CoinCounter.Names.Length];
        public int Unknown;
        public string Note = "";
    }
    static class CoinCounter
    {
        public static readonly string[] Names = { "5 cents", "10 cents", "25 cents", "1 Peso", "5 Pesos" };
        public static readonly int[] Centavos = { 5, 10, 25, 100, 500 };
        public static readonly Color[] Colors =
        {
            Color.FromArgb(255, 140, 0),
            Color.FromArgb(30, 144, 255),
            Color.FromArgb(34, 177, 76),
            Color.FromArgb(220, 20, 60),
            Color.FromArgb(148, 0, 211)
        };

        const float BoxOpacity = 0.3f;
        const double HoleRatio = 0.02;      // hole must be >= 2% of the coin's filled area
        const double SizeGap = 1.06;

        static readonly double[] RefMm = { 17.0, 20.0, 24.0, 27.0 };
        const double RefMm5c = 15.5;

        class Coin
        {
            public int Area, Filled, X0, Y0, X1, Y1;
            public double Diameter;
            public bool Holed;
            public int Denom = -1;
        }

        public static CoinResult Analyze(Bitmap src)
        {
            int w = src.Width, h = src.Height;
            CoinResult result = new CoinResult();

            // Grayscale + Otsu mask
            byte[] gray = new byte[w * h];
            for (int y = 0; y < h; y++)
                for (int x = 0; x < w; x++)
                {
                    Color c = src.GetPixel(x, y);
                    gray[y * w + x] = (byte)((c.R + c.G + c.B) / 3);
                }

            int T = Otsu(gray);
            bool[] mask = new bool[w * h];
            int on = 0;
            for (int i = 0; i < mask.Length; i++)
            {
                mask[i] = gray[i] <= T;     // coins are darker than the paper
                if (mask[i]) on++;
            }
            if (on > mask.Length / 2)
                for (int i = 0; i < mask.Length; i++) mask[i] = !mask[i];

            // Regions and Measurements
            List<Coin> coins = FindRegions(mask, w, h);


            if (coins.Count > 0)
            {
                List<int> sizes = new List<int>();
                foreach (Coin c in coins) sizes.Add(c.Filled);
                sizes.Sort();
                int median = sizes[sizes.Count / 2];
                coins.RemoveAll(delegate (Coin c) { return c.Filled < 0.25 * median || c.Filled > 5.0 * median; });
            }

            foreach (Coin c in coins)
            {
                c.Diameter = 2.0 * Math.Sqrt(c.Filled / Math.PI);
                c.Holed = (c.Filled - c.Area) >= HoleRatio * c.Filled;
            }

            // Classify
            ClassifyBySize(coins, result);

            foreach (Coin c in coins)
            {
                if (c.Denom >= 0) result.Counts[c.Denom]++;
                else result.Unknown++;
            }

            // Overlay
            result.Image = new Bitmap(w, h, PixelFormat.Format32bppArgb);
            using (Graphics g = Graphics.FromImage(result.Image))
            {
                g.DrawImage(src, 0, 0, w, h);
                foreach (Coin c in coins)
                {
                    Color col = c.Denom >= 0 ? Colors[c.Denom] : Color.Gray;
                    Rectangle box = new Rectangle(c.X0 - 2, c.Y0 - 2, c.X1 - c.X0 + 5, c.Y1 - c.Y0 + 5);
                    using (SolidBrush fill = new SolidBrush(Color.FromArgb((int)(255 * BoxOpacity), col)))
                        g.FillRectangle(fill, box);
                    using (Pen edge = new Pen(col, 2f))
                        g.DrawRectangle(edge, box);
                }
            }

            if (coins.Count == 0) result.Note = "No coins found.";
            return result;
        }

        // Otsu: threshold that best splits the histogram in two
        static int Otsu(byte[] gray)
        {
            long[] hist = new long[256];
            foreach (byte b in gray) hist[b]++;

            long total = gray.Length;
            double sumAll = 0;
            for (int t = 0; t < 256; t++) sumAll += (double)t * hist[t];

            long wB = 0;
            double sumB = 0, best = -1;
            int threshold = 0;
            for (int t = 0; t < 256; t++)
            {
                wB += hist[t];
                if (wB == 0) continue;
                long wF = total - wB;
                if (wF == 0) break;
                sumB += (double)t * hist[t];
                double mB = sumB / wB;
                double mF = (sumAll - sumB) / wF;
                double between = (double)wB * wF * (mB - mF) * (mB - mF);
                if (between > best) { best = between; threshold = t; }
            }
            return threshold;
        }

        // 4-connectivity, Explicit stack
        static List<Coin> FindRegions(bool[] mask, int w, int h)
        {
            List<Coin> coins = new List<Coin>();
            bool[] seen = new bool[mask.Length];
            int[] stack = new int[mask.Length];

            for (int start = 0; start < mask.Length; start++)
            {
                if (!mask[start] || seen[start]) continue;

                List<int> px = new List<int>();
                int sp = 0;
                stack[sp++] = start;
                seen[start] = true;
                int minX = w, maxX = -1, minY = h, maxY = -1;

                while (sp > 0)
                {
                    int i = stack[--sp];
                    px.Add(i);
                    int x = i % w, y = i / w;
                    if (x < minX) minX = x;
                    if (x > maxX) maxX = x;
                    if (y < minY) minY = y;
                    if (y > maxY) maxY = y;

                    if (x > 0 && mask[i - 1] && !seen[i - 1]) { seen[i - 1] = true; stack[sp++] = i - 1; }
                    if (x < w - 1 && mask[i + 1] && !seen[i + 1]) { seen[i + 1] = true; stack[sp++] = i + 1; }
                    if (y > 0 && mask[i - w] && !seen[i - w]) { seen[i - w] = true; stack[sp++] = i - w; }
                    if (y < h - 1 && mask[i + w] && !seen[i + w]) { seen[i + w] = true; stack[sp++] = i + w; }
                }

                int bh = maxY - minY + 1;
                int[] rowMin = new int[bh], rowMax = new int[bh];
                for (int r = 0; r < bh; r++) { rowMin[r] = int.MaxValue; rowMax[r] = -1; }
                foreach (int i in px)
                {
                    int r = i / w - minY, x = i % w;
                    if (x < rowMin[r]) rowMin[r] = x;
                    if (x > rowMax[r]) rowMax[r] = x;
                }
                int filled = 0;
                for (int r = 0; r < bh; r++)
                    if (rowMax[r] >= 0) filled += rowMax[r] - rowMin[r] + 1;

                Coin coin = new Coin();
                coin.Area = px.Count;
                coin.Filled = filled;
                coin.X0 = minX; coin.X1 = maxX; coin.Y0 = minY; coin.Y1 = maxY;
                coins.Add(coin);
            }
            return coins;
        }

        // Classification
        static void ClassifyBySize(List<Coin> coins, CoinResult result)
        {
            // Coins with a hole are 5 cents
            List<Coin> plain = new List<Coin>();
            double holedSum = 0;
            int holedCount = 0;
            foreach (Coin c in coins)
            {
                if (c.Holed) { c.Denom = 0; holedSum += c.Diameter; holedCount++; }
                else plain.Add(c);
            }
            if (plain.Count == 0) return;

            // group coins by diameter
            plain.Sort(delegate (Coin a, Coin b) { return a.Diameter.CompareTo(b.Diameter); });
            List<List<Coin>> groups = new List<List<Coin>>();
            groups.Add(new List<Coin>());
            groups[0].Add(plain[0]);
            for (int i = 1; i < plain.Count; i++)
            {
                if (plain[i].Diameter / plain[i - 1].Diameter > SizeGap) groups.Add(new List<Coin>());
                groups[groups.Count - 1].Add(plain[i]);
            }

            int m = groups.Count;
            if (m > 4)
            {
                result.Note = "More than 4 size groups found, the threshold may have split or merged coins.";
                return;
            }

            double[] means = new double[m];
            for (int j = 0; j < m; j++)
            {
                double s = 0;
                foreach (Coin c in groups[j]) s += c.Diameter;
                means[j] = s / groups[j].Count;
            }

            double bestErr = double.MaxValue;
            int bestBits = 0;
            for (int bits = 1; bits < 16; bits++)
            {
                int[] pick = new int[4];
                int n = 0;
                for (int d = 0; d < 4; d++)
                    if (((bits >> d) & 1) == 1) { if (n < 4) pick[n] = d; n++; }
                if (n != m) continue;

                List<double> r = new List<double>();
                for (int j = 0; j < m; j++) r.Add(means[j] / RefMm[pick[j]]);
                if (holedCount > 0) r.Add((holedSum / holedCount) / RefMm5c);

                double sr = 0, srr = 0;
                foreach (double v in r) { sr += v; srr += v * v; }
                double scale = sr / srr;                                        
                double err = 0;
                foreach (double v in r) err += (scale * v - 1) * (scale * v - 1);
                if (err < bestErr) { bestErr = err; bestBits = bits; }
            }

            int g = 0;
            for (int d = 0; d < 4 && g < m; d++)
            {
                if (((bestBits >> d) & 1) == 1)
                {
                    foreach (Coin c in groups[g]) c.Denom = d + 1;
                    g++;
                }
            }
        }
    }
}
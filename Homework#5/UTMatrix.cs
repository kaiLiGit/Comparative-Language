using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UTMatrix
{
	// This iterator iterates over the upper triangular matrix.
	// This is done in a row major fashion, starting with [0][0],
	// and includes all N*N elements of the matrix.
	public class UTMatrixEnumerator : IEnumerator
	{
		public UTMatrix matrix;
		public int row;
		public int col;
		
		public UTMatrixEnumerator(UTMatrix mat)
		{
			this.matrix = mat;
			Reset();
		}

		public void Reset()
		{
			row = 0;
			col = -1;
		}

		public bool MoveNext()
		{
			col++;
			
			if (col >= matrix.getSize())
			{
				row++;
				col = 0;
			}
			
			return row < matrix.getSize() && col < matrix.getSize();
		}

		object IEnumerator.Current
		{
			get
			{
				return Current;
			}
		}

		public int Current
		{
			get
			{
				try
				{
					return matrix.get(row, col);
				}
				catch (IndexOutOfRangeException)
				{
					throw new InvalidOperationException();
				}
			}
		}
	}

	public class UTMatrix : IEnumerable
	{
		// Must use a one dimensional array, having minumum size.
		public int[] data;

		// Construct an NxN Upper Triangular Matrix, initialized to 0
		// Throws an error if N is non-sensical.
		public UTMatrix(int N)
		{
			if (N <= 0)
			{
				throw new ArgumentException("Matrix size must be greater than zero");
			}
			
			data = new int[(N * (N + 1)) / 2];
		}

		// Returns the size of the matrix
		public int getSize()
		{
			return Convert.ToInt32(Math.Sqrt(8*data.Length+1)-1)/2;
		}

		// Returns an upper triangular matrix that is the summation of a & b.
		// Throws an error if a and b are incompatible.
		public static UTMatrix operator +(UTMatrix a, UTMatrix b)
		{
			if (a.getSize() != b.getSize())
			{
				throw new ArgumentException("Cannot add matrices of different dimensions");
			}
			
			UTMatrix summed = new UTMatrix(a.getSize());
			for (int row = 0; row < a.getSize(); row++)
			{
				for (int col = row; col < a.getSize(); col++)
				{
					summed.set(row, col, (a.get(row, col) + b.get(row, col)));
				}
			}
			
			return summed;
		}

		// Set the value at index [r][c] to val.
		// Throws an error if [r][c] is an invalid index to alter.
		public void set(int r, int c, int val)
		{
			if ((r >= getSize() || r < 0) && (c >= getSize() || c < 0))
				{
					throw new IndexOutOfRangeException("Index out of bounds --> row: " + r + ", col: " + c);
				}

			else if (r >= getSize() || r < 0)
				{
					throw new IndexOutOfRangeException("Index out of bounds --> row: " + r);
				}

			else if (c >= getSize() || c < 0)
				{
					throw new IndexOutOfRangeException("Index out of bounds --> col: " + c);
				}

			else if (r > c)
				{
					throw new FieldAccessException("Cannot change value at index --> row: " + r + ", col: " + c);
				}

			data[accessFunc(r, c)] = val;
		}

		// Returns the value at index [r][c]
		// Throws an error if [r][c] is an invalid index
		public int get(int r, int c)
		{
			if ((r >= getSize() || r < 0) && (c >= getSize() || c < 0))
				{
					throw new IndexOutOfRangeException("Index out of bounds --> row: " + r + ", col: " + c);
				}

			else if (r >= getSize() || r < 0)
				{
					throw new IndexOutOfRangeException("Index out of bounds --> row: " + r);
				}

			else if (c >= getSize() || c < 0)
				{
					throw new IndexOutOfRangeException("Index out of bounds --> col: " + c);
				}

			else if (r > c)
				{
					return 0;
				}
			
			return data[accessFunc(r, c)];
		}

		// Returns the position in the 1D array for [r][c].
		// Throws an error if [r][c] is an invalid index
		public int accessFunc(int r, int c)
		{
			if (r > c || r >= getSize() || c >= getSize() || r < 0 || c < 0)
				{
					throw new IndexOutOfRangeException("Invalid index, position does not exist in 1D array");
				}
			
			return (r * getSize()) + (c - ((r * (r + 1)) / 2));
		}

		// Returns an enumerator for an upper triangular matrix
		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}

		public UTMatrixEnumerator GetEnumerator()
		{
			return new UTMatrixEnumerator(this);
		}

		public static void Main(String[] args)
		{
			
			const int N = 5;
			UTMatrix ut1 = new UTMatrix(N);
			UTMatrix ut2 = new UTMatrix(N);
			for (int r=0; r<N; r++) {
				ut1.set(r, r, 1);
				for (int c=r; c<N; c++) {
					ut2.set(r, c, 1);
				}
			}
			UTMatrix ut3 = ut1 + ut2;
			UTMatrixEnumerator ie = ut3.GetEnumerator();
			while (ie.MoveNext()) {
				Console.Write(ie.Current + " ");
			}
			Console.WriteLine();
			foreach (int v in ut3) {
				Console.Write(v + " ");
			}
			Console.WriteLine();

		}
	}
}
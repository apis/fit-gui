using System;
using System.Drawing;
using System.Windows.Forms;

namespace fit.gui
{
	public class ProgressBar : UserControl
	{
		private System.ComponentModel.Container components = null;

		private int internalStep = 10;

		public int Step
		{
			get
			{
				return internalStep;
			}
			set
			{
				internalStep = value;
			}
		}

		private Color color = Color.Navy;

		public Color Color
		{
			get
			{
				return color;
			}

			set
			{
				color = value;

				Invalidate();
			}
		}

		public void PerformStep()
		{
			Increment(internalStep);
		}

		public void Increment(int value)
		{
			Value += value;
		}

		protected override void OnResize(EventArgs e)
		{
			Invalidate();
		}

		protected override void OnPaint(PaintEventArgs eventArgs)
		{
			Graphics graphics = eventArgs.Graphics;

			using (SolidBrush brush = new SolidBrush(color))
			{
				float percent = (float)(internalValue - internalMinimum) / (float)(internalMaximum - internalMinimum);
				Rectangle rectangle = ClientRectangle;

				rectangle.Width = (int)((float)rectangle.Width * percent);

				graphics.FillRectangle(brush, rectangle);

				Draw3DBorder(graphics);
			}
		}

		private int internalMinimum = 0;

		public int Minimum
		{
			get
			{
				return internalMinimum;
			}

			set
			{
				if (value < 0)
				{
					internalMinimum = 0;
				}
				else
				{
					if (value > internalMaximum)
					{
						internalMinimum = internalMaximum;
					}
					else
					{
						internalMinimum = value;
					}
				}

				FixValueIfLessThanMinimum();
				Invalidate();
			}
		}

		private void FixValueIfLessThanMinimum()
		{
			if (internalValue < internalMinimum)
			{
				internalValue = internalMinimum;
			}
		}

		private int internalMaximum = 100;

		public int Maximum
		{
			get
			{
				return internalMaximum;
			}

			set
			{
				if (value < internalMinimum)
				{
					internalMaximum = internalMinimum;
				}
				else
				{
					internalMaximum = value;
				}

				FixValueIfMoreThanMaximum();
				Invalidate();
			}
		}

		private void FixValueIfMoreThanMaximum()
		{
			if (internalValue > internalMaximum)
			{
				internalValue = internalMaximum;
			}
		}

		private int internalValue = 0;

		public int Value
		{
			get
			{
				return internalValue;
			}

			set
			{
				int oldInternalValue = internalValue;

				if (value < internalMinimum)
				{
					internalValue = internalMinimum;
				}
				else
				{
					if (value > internalMaximum)
					{
						internalValue = internalMaximum;
					}
					else
					{
						internalValue = value;
					}
				}

				InvalidateChangedArea(oldInternalValue, internalValue);
			}
		}

		private void InvalidateChangedArea(int oldValue, int newValue)
		{
			float percent;

			Rectangle newValueRect = ClientRectangle;
			Rectangle oldValueRect = ClientRectangle;

			percent = (float)(newValue - internalMinimum) / (float)(internalMaximum - internalMinimum);
			newValueRect.Width = (int)((float)newValueRect.Width * percent);

			percent = (float)(oldValue - internalMinimum) / (float)(internalMaximum - internalMinimum);
			oldValueRect.Width = (int)((float)oldValueRect.Width * percent);

			Rectangle updateRect = new Rectangle();

			if (newValueRect.Width > oldValueRect.Width)
			{
				updateRect.X = oldValueRect.Size.Width;
				updateRect.Width = newValueRect.Width - oldValueRect.Width;
			}
			else
			{
				updateRect.X = newValueRect.Size.Width;
				updateRect.Width = oldValueRect.Width - newValueRect.Width;
			}

			updateRect.Height = Height;

			Invalidate(updateRect);
		}

		private void Draw3DBorder(Graphics g)
		{
			int PenWidth = (int)Pens.White.Width;

			g.DrawLine(Pens.DarkGray,
				new Point(this.ClientRectangle.Left, this.ClientRectangle.Top),
				new Point(this.ClientRectangle.Width - PenWidth, this.ClientRectangle.Top));
			g.DrawLine(Pens.DarkGray,
				new Point(this.ClientRectangle.Left, this.ClientRectangle.Top),
				new Point(this.ClientRectangle.Left, this.ClientRectangle.Height - PenWidth));
			g.DrawLine(Pens.White,
				new Point(this.ClientRectangle.Left, this.ClientRectangle.Height - PenWidth),
				new Point(this.ClientRectangle.Width - PenWidth, this.ClientRectangle.Height - PenWidth));
			g.DrawLine(Pens.White,
				new Point(this.ClientRectangle.Width - PenWidth, this.ClientRectangle.Top),
				new Point(this.ClientRectangle.Width - PenWidth, this.ClientRectangle.Height - PenWidth));
		}

		public ProgressBar()
		{
			InitializeComponent();
		}

		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				if (components != null)
				{
					components.Dispose();
				}
			}
			base.Dispose(disposing);
		}

		private void InitializeComponent()
		{
			components = new System.ComponentModel.Container();
		}
	}
}

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WpfDiary.Views
{
    /// <summary>
    /// Follow steps 1a or 1b and then 2 to use this custom control in a XAML file.
    ///
    /// Step 1a) Using this custom control in a XAML file that exists in the current project.
    /// Add this XmlNamespace attribute to the root element of the markup file where it is 
    /// to be used:
    ///
    ///     xmlns:MyNamespace="clr-namespace:WpfDiary.Views"
    ///
    ///
    /// Step 1b) Using this custom control in a XAML file that exists in a different project.
    /// Add this XmlNamespace attribute to the root element of the markup file where it is 
    /// to be used:
    ///
    ///     xmlns:MyNamespace="clr-namespace:WpfDiary.Views;assembly=WpfDiary.Views"
    ///
    /// You will also need to add a project reference from the project where the XAML file lives
    /// to this project and Rebuild to avoid compilation errors:
    ///
    ///     Right click on the target project in the Solution Explorer and
    ///     "Add Reference"->"Projects"->[Browse to and select this project]
    ///
    ///
    /// Step 2)
    /// Go ahead and use your control in the XAML file.
    ///
    ///     <MyNamespace:DateSlider/>
    ///
    /// </summary>
    public class DateSlider : Control
    {
        public static readonly DependencyProperty Start = DependencyProperty.Register("StartDate", typeof(DateTime), typeof(DateSlider), new FrameworkPropertyMetadata(new DateTime(2010, 1, 1)));
        public static readonly DependencyProperty End = DependencyProperty.Register("EndDate", typeof(DateTime), typeof(DateSlider), new FrameworkPropertyMetadata(new DateTime(DateTime.Now.Year + 1, 1, 1)));
        public static readonly DependencyProperty DateList = DependencyProperty.Register("Dates", typeof(List<DateTime>), typeof(DateSlider), new FrameworkPropertyMetadata(new List<DateTime>()));

        public DateTime StartDate
        {
            get
            {
                return (DateTime)GetValue(Start);
            }
            set
            {
                SetValue(Start, value);
            }
        }

        public DateTime EndDate
        {
            get
            {
                return (DateTime)GetValue(End);
            }
            set
            {
                SetValue(End, value);
            }
        }

        public List<DateTime> Dates
        {
            get
            {
                return (List<DateTime>)GetValue(DateList);
            }
            set
            {
                SetValue(DateList, value);
            }
        }

        private DateTime draggedDate;

        public string IncludedRangeColor;
        public string ExcludedRangeColor;
        public string OutlineColor;

        private static BrushConverter converter = new BrushConverter();

        private Pen includedRangePen;
        private Pen excludedRangePen;
        private Pen outlinePen;

        private int monthsDelta;
        private const int THUMB_RADIUS = 4;

        private const int FONT_SIZE = 12;
        private const int MIN_FONT_SIZE = 8;
        private const int REFERENCE_WIDTH = 600;

        private double lastDrawnTextX = 0;
        private double lastDrawnHeight;

        public int TextSize
        {
            get { return (int)Math.Max(Math.Min(FONT_SIZE, FONT_SIZE * ActualWidth / REFERENCE_WIDTH), MIN_FONT_SIZE); }
        }
        static DateSlider()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(DateSlider), new FrameworkPropertyMetadata(typeof(DateSlider)));
        }

        public DateSlider()
        {
            IncludedRangeColor = "#d10208";
            ExcludedRangeColor = "#b2b2b2";
            OutlineColor = "#c40101";

            includedRangePen = new Pen((Brush)converter.ConvertFromString(IncludedRangeColor), 2.0);
            excludedRangePen = new Pen((Brush)converter.ConvertFromString(ExcludedRangeColor), 2.0);
            outlinePen = new Pen((Brush)converter.ConvertFromString(OutlineColor), 2.0);

            PreviewMouseLeftButtonDown += StartDragging;
            PreviewMouseLeftButtonUp += StopDragging;
            PreviewMouseRightButtonDown += ToggleDate;

            this.Loaded += Init;
        }

        void Init(object sender, RoutedEventArgs e)
        {
            monthsDelta = MonthsBetween(StartDate, EndDate);
            if (Dates.Count == 0)
            {
                Dates.Add(StartDate);
                Dates.Add(EndDate);
            }
            InvalidateVisual();
        }

        protected override void OnRender(DrawingContext drawingContext)
        {
            base.OnRender(drawingContext);

            List<Point> thumbs = new List<Point>();

            IEnumerator<DateTime> iterator = Dates.GetEnumerator();
            iterator.MoveNext();
            DateTime first = iterator.Current;

            Point firstPoint = new Point(DateToXPoint(first), ActualHeight / 2);
            thumbs.Add(firstPoint);

            drawingContext.DrawLine(excludedRangePen, new Point(0, ActualHeight / 2), firstPoint);
            drawingContext.DrawEllipse(excludedRangePen.Brush, excludedRangePen, firstPoint, 4, 4);

            lastDrawnTextX = 0;
            DrawDateText(drawingContext, StartDate, 0);
            DrawDateText(drawingContext, first, firstPoint.X);

            DateTime lastDate = first;

            bool includedTurn = true;
            while(iterator.MoveNext())
            {
                Pen p = includedTurn ? includedRangePen : excludedRangePen;
                Point curPoint = new Point(DateToXPoint(iterator.Current), ActualHeight / 2);
                thumbs.Add(curPoint);
                drawingContext.DrawLine(p, new Point(DateToXPoint(lastDate), ActualHeight / 2), curPoint);

                DrawDateText(drawingContext, iterator.Current, curPoint.X);

                lastDate = iterator.Current;
                includedTurn = !includedTurn;
            }

            drawingContext.DrawLine(excludedRangePen, new Point(DateToXPoint(lastDate), ActualHeight / 2), new Point(ActualWidth, ActualHeight / 2));

            DrawDateText(drawingContext, EndDate, ActualWidth);

            foreach(Point thumb in thumbs)
            {
                drawingContext.DrawEllipse(includedRangePen.Brush, outlinePen, thumb, THUMB_RADIUS, THUMB_RADIUS);
            }
        }

        private void ToggleDate(object sender, MouseButtonEventArgs e)
        {
            DateTime clickedDate = XPointToDate(e.GetPosition(this).X);

            if (Dates.FirstOrDefault(date => DatesEqualWithMonthAccuracy(date, clickedDate)) == DateTime.MinValue)
            {
                Dates.Add(clickedDate);
            }
            else
            {
                Dates.Remove(clickedDate);
            }
            Dates.Sort();
            InvalidateVisual();
        }

        private void StartDragging(object sender, MouseButtonEventArgs e)
        {
            DateTime clickedDate = XPointToDate(e.GetPosition(this).X);
            Console.WriteLine(clickedDate);
            draggedDate = Dates.FirstOrDefault(date => DatesEqualWithMonthAccuracy(date, clickedDate));
            if (!draggedDate.Equals(DateTime.MinValue))
            {
                PreviewMouseMove += HandleDrag;
            }
        }

        private void HandleDrag(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                DateTime targetDate = XPointToDate(e.GetPosition(this).X);
                if (!(DatesEqualWithMonthAccuracy(targetDate, draggedDate)))
                {
                    Dates.Remove(draggedDate);
                    draggedDate = targetDate;
                    Dates.Add(targetDate);
                    Dates.Sort();
                    InvalidateVisual();
                }
            }
            else
            {
                PreviewMouseMove -= HandleDrag;
            }
        }

        private void StopDragging(object sender, MouseButtonEventArgs e)
        {
            draggedDate = DateTime.MinValue;
            PreviewMouseMove -= HandleDrag;
        }

        private DateTime XPointToDate(double x)
        {
            double XDelta = ActualWidth / monthsDelta;
            return StartDate.AddMonths((int)Math.Round(x / XDelta));
        }

        private double DateToXPoint(DateTime date)
        {
            double XDelta = ActualWidth / monthsDelta;
            return XDelta * MonthsBetween(StartDate, date);
        }

        private int MonthsBetween(DateTime start, DateTime end)
        {
            return ((end.Year - start.Year) * 12) + end.Month - StartDate.Month;
        }

        private void DrawDateText(DrawingContext ctx, DateTime date, double x)
        {
            FormattedText ft = new FormattedText(date.ToString("MM'/'yy"), CultureInfo.CurrentCulture, FlowDirection.LeftToRight, new Typeface(new FontFamily("Segoe UI"), FontStyles.Normal,
                                        FontWeights.Normal,
                                        FontStretches.Normal), TextSize, Brushes.Black);
            if (DatesEqualWithMonthAccuracy(date, StartDate) || DatesEqualWithMonthAccuracy(date, EndDate) || lastDrawnTextX < x - ft.WidthIncludingTrailingWhitespace / 2)
            {
                lastDrawnHeight = ActualHeight / 1.2;
            }
            else
            {
                lastDrawnHeight = lastDrawnHeight + ft.Height;
            }
            ctx.DrawText(ft, new Point(x - ft.WidthIncludingTrailingWhitespace / 2, lastDrawnHeight));
            lastDrawnTextX = x + ft.WidthIncludingTrailingWhitespace / 2;
        }

        private static bool DatesEqualWithMonthAccuracy(DateTime first, DateTime second)
        {
            return first.Year.Equals(second.Year) && first.Month.Equals(second.Month);
        }
    }
}

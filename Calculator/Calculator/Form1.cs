using System;
using System.Linq;
using System.Windows.Forms;

namespace Calculator
{
    public partial class Form1 : Form
    {
        #region Constants

        private const string Numbers = "0123456789";

        #endregion

        #region Fields

        private double _value = 0;
        private bool _operationClicked = false;
        private bool _equalsClicked = false;
        private bool _decimalClicked = false;
        private string _lastOperation = string.Empty;
        private string _currentOperation = string.Empty;

        #endregion

        public Form1()
        {
            InitializeComponent();
            this.KeyPreview = true;
            this.KeyPress += Form1_KeyPress;

            //decomment for tests
            //this.Load += Form1_Load;
        }


        private void Form1_KeyPress(object sender, KeyPressEventArgs e)
        {
            char keyChar = e.KeyChar;

            if (Numbers.Contains(keyChar))
            {
                HandleNumberInsert(keyChar.ToString());
            }


            //TODO: handle for operations and equals
        }

        private void Number_Click(object sender, EventArgs e)
        {
            Button b = (Button)sender;

            //eliminate the front 0 and introduce new numbers
            HandleNumberInsert(b.Text);
        }

        /// <summary>
        /// Allows to insert numbers from keyboard and click on buttons
        /// </summary>
        private void HandleNumberInsert(string text)
        {
            if (result.Text == "0" || _operationClicked)
            {
                result.Text = text;
            }
            else //allow multi-digit numbers
            {
                result.Text = result.Text + text;
            }

            _operationClicked = false;
            _decimalClicked = false;
        }

        private double PerformOperation(string operation)
        {
            double calculationResult = 0;
            switch (operation)
            {
                case "+":
                    calculationResult = _value + double.Parse(result.Text);
                    break;
                case "-":
                    calculationResult = _value - double.Parse(result.Text);
                    break;
                case "*":
                    calculationResult = _value * double.Parse(result.Text);
                    break;
                case "/":
                    try
                    {
                        calculationResult = _value / double.Parse(result.Text);
                    }
                    catch (DivideByZeroException)
                    {
                        result.Text = "Cannot divide by zero";
                        preCalculateLabel.Text = "";
                    }
                    break;
                default:
                    break;
            }
            return calculationResult;
        }
        private void Operation_Click(object sender, EventArgs e)
        {
            _lastOperation = _currentOperation;

            Button b = (Button)sender;
            _currentOperation = b.Text;

            //first operation added to the label or after pressing =
            if (string.IsNullOrEmpty(preCalculateLabel.Text) || _equalsClicked)
            {
                preCalculateLabel.Text = $"{result.Text} {_currentOperation}";
                _value = double.Parse(result.Text);
                result.Text = "0";
            }
            // if operation mistake, change label
            else if (_operationClicked)
            {
                preCalculateLabel.Text = preCalculateLabel.Text.Substring(0, preCalculateLabel.Text.Length - 1) + _currentOperation;
            }
            //update label
            else
            {
                preCalculateLabel.Text = preCalculateLabel.Text + " " + result.Text + " " + _currentOperation;
                if (!_equalsClicked || _decimalClicked)
                {
                    //when operation clicked, update the result with the operation before
                    _value = PerformOperation(_lastOperation);
                }
            }

            _equalsClicked = false;
            result.Text = _value.ToString();
            _operationClicked = true;

        }
        private void Equals_Click(object sender, EventArgs e)
        {
            _equalsClicked = true;
            preCalculateLabel.Text = "";

            result.Text = PerformOperation(_currentOperation).ToString();

            _operationClicked = false;
        }
        private void ButtonDecimal_Click(object sender, EventArgs e)
        {
            Button b = (Button)sender;

            if (int.TryParse(result.Text, out int number))
            {
                result.Text = result.Text + b.Text;
            }

            if (_operationClicked || _equalsClicked)
            {
                result.Text = "0" + b.Text;
            }

            _decimalClicked = true;
            _operationClicked = false;
            _equalsClicked = false;
        }
        private void ButtonNegative_Click(object sender, EventArgs e)
        {
            result.Text = (double.Parse(result.Text) * (-1)).ToString();
        }
        private void Clear_Click(object sender, EventArgs e)
        {
            result.Text = "0";
            preCalculateLabel.Text = string.Empty;
            _value = 0;
            _lastOperation = string.Empty;
            _currentOperation = string.Empty;
        }
        private void ClearEntry_Click(object sender, EventArgs e)
        {
            result.Text = "0";
        }
        private void Delete_Click(object sender, EventArgs e)
        {
            if (int.TryParse(result.Text, out int number))
            {
                result.Text = (number / 10).ToString();
            }
            else
            {
                result.Text = result.Text.Substring(0, result.Text.Length - 1);
            }

        }

        #region Tests

        private void Form1_Load(object sender, EventArgs e)
        {
            TestForMinus();
        }

        private void TestForMinus()
        {
            this.button7.PerformClick();
            this.Substract.PerformClick();

            if (preCalculateLabel.Text != "7 -")
            {
                throw new Exception("nu e bine la ceva");
            }
        }

        #endregion Tests


    }
}

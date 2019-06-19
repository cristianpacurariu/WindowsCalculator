using System;
using System.Windows.Forms;

namespace Calculator
{
    public partial class Form1 : Form
    {
        #region Constants

        private const string Numbers = "0123456789";
        private const string ListOfOperations = "+-*/";

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
            string keyPressed = e.KeyChar.ToString();

            //MessageBox.Show("you have pressed " + keyPressed);

            //number pressed
            if (Numbers.Contains(keyPressed))
            {
                HandleNumberInsert(keyPressed);
            }

            //operation pressed
            if (ListOfOperations.Contains(keyPressed))
            {
                HandleOperationInsert(keyPressed);
            }

            // equals pressed or ENTER
            if (keyPressed == "=" || e.KeyChar == (char)Keys.Enter)
            {
                _equalsClicked = true;
                preCalculateLabel.Text = "";

                result.Text = PerformOperation(_currentOperation).ToString();

                _operationClicked = false;
            }

            //decimal pressed
            if (keyPressed == ".")
            {
                HandleDecimal();
            }

            //escape pressed
            if (e.KeyChar == (char)Keys.Escape)
            {
                ClearAll();
            }

            //backspace pressed
            if (e.KeyChar == (char)Keys.Back)
            {
                EraseLastDigit();
            }
        }

        private void HandleNumberInsert(string text)
        {
            if (result.Text == "0" || _operationClicked || _equalsClicked)
            {
                result.Text = text;
            }
            else //allow multi-digit numbers
            {
                result.Text = result.Text + text;
            }

            _operationClicked = false;
            _decimalClicked = false;
            _equalsClicked = false;
        }
        private void HandleOperationInsert(string operation)
        {
            _currentOperation = operation;
            //first operation added to the label or after pressing =
            if (string.IsNullOrEmpty(preCalculateLabel.Text) || _equalsClicked)
            {
                preCalculateLabel.Text = $"{result.Text} {operation}";
                _value = double.Parse(result.Text);
                result.Text = "0";
            }
            // if operation mistake, change label
            else if (_operationClicked)
            {
                preCalculateLabel.Text = preCalculateLabel.Text.Substring(0, preCalculateLabel.Text.Length - 1) + operation;
            }
            //update label
            else
            {
                preCalculateLabel.Text = preCalculateLabel.Text + " " + result.Text + " " + operation;
                if (!_equalsClicked || _decimalClicked)
                {
                    //when operation clicked, update the result with the operation before
                    _value = PerformOperation(_lastOperation);
                }
            }
            _equalsClicked = false;
            result.Text = _value.ToString();
            _operationClicked = true;
            _lastOperation = _currentOperation;
        }
        private void HandleDecimal()
        {
            if (int.TryParse(result.Text, out int number))
            {
                result.Text = result.Text + ".";
            }

            if (_operationClicked || _equalsClicked)
            {
                result.Text = "0" + ".";
            }

            _decimalClicked = true;
            _operationClicked = false;
            _equalsClicked = false;
        }
        private void ClearAll()
        {
            result.Text = "0";
            preCalculateLabel.Text = string.Empty;
            _value = 0;
            _lastOperation = string.Empty;
            _currentOperation = string.Empty;
        }
        private void EraseLastDigit()
        {
            result.Text = result.Text.Substring(0, result.Text.Length - 1);
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
        /// <summary>
        /// Allows to insert operations from the keyboard and click on them through the interface    
        /// </summary>
        private void Operation_Click(object sender, EventArgs e)
        {
            Button b = (Button)sender;
            _currentOperation = b.Text;

            HandleOperationInsert(_currentOperation);
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
            HandleDecimal();
        }
        private void ButtonNegative_Click(object sender, EventArgs e)
        {
            result.Text = (double.Parse(result.Text) * (-1)).ToString();
        }
        private void Clear_Click(object sender, EventArgs e)
        {
            ClearAll();
        }
        private void ClearEntry_Click(object sender, EventArgs e)
        {
            result.Text = "0";
        }
        private void Delete_Click(object sender, EventArgs e)
        {
            EraseLastDigit();
        }

        #region Tests

        private void Form1_Load(object sender, EventArgs e)
        {
            TestForNumberInsertion();
            TestForOperationInsertion();
            TestForDecimalPoint();
            TestForEqualsInsertion();
            TestForClearAll();
            TestForEraseLastDigit();
        }
        private void TestForEraseLastDigit()
        {
            throw new NotImplementedException();
        }
        private void TestForClearAll()
        {
            throw new NotImplementedException();
        }
        private void TestForEqualsInsertion()
        {
            throw new NotImplementedException();
        }
        private void TestForDecimalPoint()
        {
            throw new NotImplementedException();
        }
        private void TestForOperationInsertion()
        {
            throw new NotImplementedException();
        }
        private void TestForNumberInsertion()
        {
            //initial insertion
            //double digit = this.button7.Click;

            //inserting multidigit numbers
        }

        #endregion Tests
    }
}

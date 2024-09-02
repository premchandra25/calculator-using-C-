using System;
using System.Linq.Expressions;
using System.Security.AccessControl;
using System.Numerics;

namespace calculator
{
    public partial class Form1 : Form
    {


        public Form1()
        {
            InitializeComponent();


        }

        private void button4_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(textBox2.Text))
                {
                    MessageBox.Show("Please enter a valid expression.");
                    return;
                }
                String strExpression = textBox2.Text;
                double dResult = EvaluateExpression(strExpression);
                textBox2.Text = dResult.ToString();
            }

            catch (Exception ex)
            {
                Console.WriteLine("Error Exception: " + ex);
                textBox2.Text = "Error";
                Console.WriteLine(ex.StackTrace);
            }
            finally
            {
                Console.WriteLine("Evaluated, exiting the function.");
            }
        }

       

        private void button8_Click(object sender, EventArgs e)//The buttons from 0-9 if used the textbox displays the result in it
        {
            try
            {
                if (textBox2.Text == "0")
                {
                    textBox2.Clear();
                }
                Button button = (Button)sender;
                textBox2.Text = textBox2.Text + button.Text;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error Exception: " + ex);
                Console.WriteLine(ex.StackTrace);
            }
            finally
            {
                Console.WriteLine("Evaluated, exiting the function.");
            }

        }

        private void operator_click(object sender, EventArgs e)
        {
            try
            {
                Button button = (Button)sender;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error Exception: " + ex);
                Console.WriteLine(ex.StackTrace);
            }
            finally
            {
                Console.WriteLine("Evaluated, exiting the function.");
            }
        }

        private void button3_Click(object sender, EventArgs e) //The button AC if used set the result value to 0
        {
            try
            {
                textBox2.Text = "0";
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error Exception: " + ex);
                Console.WriteLine(ex.StackTrace);
            }
            finally
            {
                Console.WriteLine("Evaluated, exiting the function.");
            }
        }

        private void button20_Click(object sender, EventArgs e)//The button C if used set the result value to 0
        {
            try
            {
                textBox2.Text = "0";
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error Exception: " + ex);
                Console.WriteLine(ex.StackTrace);
            }
            finally
            {
                Console.WriteLine("Evaluated, exiting the function.");
            }
        }

        /*
		Name: evaluateExpression()
		Parameters: strExpression
		Return:	The value from the expression that is evaluated from the evaluateSimpleExpression() 
		Author: PC	
		Description: This function evaluates the expression that contains the parentheses and then solves the expressions present in the parentheses
		
		*/
        public double EvaluateExpression(string strExpression)
        {
            try
            {
                // Evaluate any expressions within parentheses first
                while (strExpression.Contains("("))
                {
                    int nOpenIndex = strExpression.LastIndexOf('(');  // Checks for the occurrence of ['('] last time in the expression
                    if (nOpenIndex == -1)
                    {
                        throw new ArgumentException("Opening parenthesis is missing.");
                    }
                    int nCloseIndex = strExpression.IndexOf(')', nOpenIndex); // Checks for the occurrence of [')'] first time in the expression
                    if (nCloseIndex == -1)
                    {
                        throw new ArgumentException("Closing parenthesis is missing.");
                    }
                    string strSubExpression = strExpression.Substring(nOpenIndex + 1, nCloseIndex - nOpenIndex - 1); // This stores the subexpression present inside the parentheses, e.g., "(3+5)" -> "3+5"
                    double dSubResult = EvaluateSimpleExpression(strSubExpression); // Calls the EvaluateSimpleExpression() for evaluating the subexpression
                    strExpression = strExpression.Substring(0, nOpenIndex) + dSubResult + strExpression.Substring(nCloseIndex + 1); // This replaces the subexpression with the evaluated value 
                }
            }
            catch (ArgumentException ae)
            {
                Console.WriteLine("Argument Error: " + ae.Message);
                
            }
            catch (Exception e)
            {
                Console.WriteLine("Error Exception: " + e);
                Console.WriteLine(e.StackTrace);
            }
            finally
            {
                Console.WriteLine("Evaluated, exiting the function.");
            }
            // After all parentheses are handled, evaluate the remaining expression
            return EvaluateSimpleExpression(strExpression); // Evaluates the simple expression which does not have any parentheses
        }

        /*
		Name: evaluateSimpleExpression()
		Parameters: strExpression
		Return:	The value from the expression that is evaluated from the evaluateOperation() or the evaluateAdditionOrSubtraction() 
		Author: PC	
		Description: This function evaluates the expression that contains the operators it checks for the operators as per the order of BODMAS and evaluates it
		
		*/
        public double EvaluateSimpleExpression(string strExpression)
        {
            double dValue = 0;
            try
            {

                // Evaluate division first
                while (strExpression.Contains("/"))
                {
                    strExpression = EvaluateOperation(strExpression, '/');
                }

                // Evaluate multiplication next
                while (strExpression.Contains("*"))
                {
                    strExpression = EvaluateOperation(strExpression, '*');
                }

                // Evaluate addition next
                while (strExpression.Contains("+") || (strExpression.Contains("-") && strExpression.LastIndexOf('-') != 0)) // strExpression.LastIndexOf('-') checks for the last occurrence of '-' operator
                {
                    strExpression = EvaluateAdditionOrSubtraction(strExpression);
                }
                dValue = double.Parse(strExpression);
            }
             
            catch (Exception e)
            {
                Console.WriteLine("Error Exception: " + e);
                Console.WriteLine(e.StackTrace);
            }
            finally
            {
                Console.WriteLine("Evaluated, exiting the function.");
            }
            return dValue;
        }

        /*
		Name: evaluatingOperation()
		Parameters: strExpression, operator
		Return:	The value from the expression that is evaluated as the expression is passed and evaluates as per the operator that is passed 
		Author: PC	
		Description: This function evaluates the expression that contains the operator that is passed and returns the expression after evaluating it.
		
		*/
        public string EvaluateOperation(string strExpression, char operatorSymbol)
        {
            String strExp = "";
            if (operatorSymbol != '+' && operatorSymbol != '-' && operatorSymbol != '*' && operatorSymbol != '/')
            {
                throw new ArgumentException("Invalid operator: " + operatorSymbol);
            }
            int nIndex = strExpression.IndexOf(operatorSymbol); // Here the nIndex stores the index of the operator that is passed
            if (nIndex == -1)
            {
                Console.WriteLine("The operator '{operatorSymbol}' is not present in the expression.");
              
                
            }
            int nLeftIndex = nIndex - 1;  // This is to calculate the left side of the operator it stores the value of it

            if (nLeftIndex < 0 || !char.IsDigit(strExpression[nLeftIndex]) && strExpression[nLeftIndex] != '.')
            {
                throw new ArgumentException("Invalid expression: No valid number on the left side of the operator.");
            }
            int nRightIndex = nIndex + 1; // This is to calculate the right side of the operator it stores the value of it

            if (nRightIndex >= strExpression.Length || !char.IsDigit(strExpression[nRightIndex]) && strExpression[nRightIndex] != '.')
            {
                throw new ArgumentException("Invalid expression: No valid number on the right side of the operator.");
            }

            try
            {
                // Find the number on the left of the operator
                while (nLeftIndex >= 0 && (char.IsDigit(strExpression[nLeftIndex]) || strExpression[nLeftIndex] == '.'))
                {
                    nLeftIndex--;
                }

                nLeftIndex++;

                // Find the number on the right of the operator
                while (nRightIndex < strExpression.Length && (char.IsDigit(strExpression[nRightIndex]) || strExpression[nRightIndex] == '.'))
                {
                    nRightIndex++;
                }

                int nLength = nIndex - nLeftIndex;
                if (nLength <= 0 || (nLeftIndex + nLength) > strExpression.Length)
                {
                    throw new ArgumentOutOfRangeException("The length for substring extraction is invalid.");
                }
                // Extract numbers from the expression
                double dLeftNumber = double.Parse(strExpression.Substring(nLeftIndex, nIndex - nLeftIndex)); // Stores the value of the left side of the operator

                if (nIndex + 1 >= strExpression.Length || nRightIndex <= nIndex || nRightIndex - nIndex - 1 < 0)
                {
                    throw new ArgumentOutOfRangeException("The calculated substring indices are out of range.");
                }
                double dRightNumber = double.Parse(strExpression.Substring(nIndex + 1, nRightIndex - nIndex - 1)); // Stores the value of the right side of the operator

                // Perform the operation
                double dResult = 0; // This dResult will be assigned zero because every evaluation of the expression we store the subexpression in the dResult and we replace it in the new expression
                switch (operatorSymbol)
                {
                    case '/':
                        if (dRightNumber == 0)
                        {
                            throw new ArithmeticException("Division by zero not possible!");
                        }
                        dResult = dLeftNumber / dRightNumber;
                        break;

                    case '*':
                        dResult = dLeftNumber * dRightNumber;
                        break;
                }

                // Replace the operation in the expression with the dResult
                strExp = strExpression.Substring(0, nLeftIndex) + dResult + strExpression.Substring(nRightIndex);

            }
            catch (ArgumentOutOfRangeException aoore)
            {
                Console.WriteLine("Range Error: " + aoore.Message);
            }
            catch (ArgumentException ae)
            {
                Console.WriteLine("Argument Error: " + ae.Message);
                
            }
           
            catch (Exception e)
            {
                Console.WriteLine("Error Exception: " + e);
                Console.WriteLine(e.StackTrace);
            }
            finally
            {
                Console.WriteLine("Evaluated, exiting the function.");
            }
            return strExp;
        }

        /*
		Name: evaluateAdditionOrSubtraction()
		Parameters: strExpression
		Return:	The value from the expression and returns the value that is obtained 
		Author: PC	
		Description: This function evaluates the expression that contains the operators '+' & '-' evaluates the expression as per operators and returns the value from the function.
		
		*/
        public string EvaluateAdditionOrSubtraction(string strExpression)
        {
            String strExp = "";
            int nIndexPlus = strExpression.IndexOf('+');  // Returns -1 if not found at any index
            if (nIndexPlus == -1)
            {
                Console.WriteLine("The operator 'operatorSymbol' is not present in the expression.");

            }
            int nIndexMinus = strExpression.IndexOf('-'); // Gives the index of '-' first occurrence
            if (nIndexMinus == -1)
            {
                Console.WriteLine("The operator 'operatorSymbol' is not present in the expression.");

            }
            if (nIndexMinus == 0) // If the '-' operator is at index 0
            {
                nIndexMinus = strExpression.IndexOf('-', 1); // This checks for the last occurrence of '-' operator and stores the index of it in nIndexMinus
                if (nIndexMinus == -1)
                {
                    Console.WriteLine("The operator 'operatorSymbol' is not present in the expression.");

                }
            }

            int nIndex = 0;  // Initialize the variable nIndex to 0

            if (nIndexPlus >= 0 && (nIndexPlus < nIndexMinus || nIndexMinus < 0)) // Check in the case of plus operator the index of the operator is assigned as nIndex
            {
                nIndex = nIndexPlus;
            }
            else
            {
                nIndex = nIndexMinus;  // Else the operator '-' index would be assigned to nIndex
            }

            int nLeftIndex = nIndex - 1;  // This is to calculate the left side of the operator it stores the value of it
            if (nLeftIndex < 0 || !char.IsDigit(strExpression[nLeftIndex]) && strExpression[nLeftIndex] != '.')
            {
                throw new ArgumentException("Invalid expression: No valid number on the left side of the operator.");
            }
            int nRightIndex = nIndex + 1;  // This is to calculate the right side of the operator it stores the value of it
            if (nRightIndex >= strExpression.Length || !char.IsDigit(strExpression[nRightIndex]) && strExpression[nRightIndex] != '.')
            {
                throw new ArgumentException("Invalid expression: No valid number on the right side of the operator.");
            }

            try
            {
                // Find the number on the left of the operator
                while (nLeftIndex >= 0 && (char.IsDigit(strExpression[nLeftIndex]) || strExpression[nLeftIndex] == '.'))
                {
                    nLeftIndex--;
                }

                if (nLeftIndex >= 0 && strExpression[nLeftIndex] == '-')
                {
                    nLeftIndex--;
                }
                nLeftIndex++;

                // Find the number on the right of the operator
                while (nRightIndex < strExpression.Length && (char.IsDigit(strExpression[nRightIndex]) || strExpression[nRightIndex] == '.'))
                {
                    nRightIndex++;
                }

                int nLength = nIndex - nLeftIndex;
                if (nLength <= 0 || (nLeftIndex + nLength) > strExpression.Length)
                {
                    throw new ArgumentOutOfRangeException("The length for substring extraction is invalid.");
                }
                // Extract numbers from the expression
                double dLeftNumber = double.Parse(strExpression.Substring(nLeftIndex, nIndex - nLeftIndex)); // Stores the value of the left side of the operator
                if (nIndex + 1 >= strExpression.Length || nRightIndex <= nIndex || nRightIndex - nIndex - 1 < 0)
                {
                    throw new ArgumentOutOfRangeException("The calculated substring indices are out of range.");
                }
                double dRightNumber = double.Parse(strExpression.Substring(nIndex + 1, nRightIndex - nIndex - 1)); // Stores the value of the right side of the operator

                // Perform the operation
                double dResult = 0;
                if (strExpression[nIndex] == '+')
                {
                    dResult = dLeftNumber + dRightNumber;
                }
                else
                {
                    dResult = dLeftNumber - dRightNumber;
                }

                // Replace the operation in the expression with the dResult
                strExp = strExpression.Substring(0, nLeftIndex) + dResult + strExpression.Substring(nRightIndex);

            }
            catch (ArgumentOutOfRangeException aoore)
            {
                Console.WriteLine("Range Error: " + aoore.Message);
            }
            catch (ArgumentException ae)
            {
                Console.WriteLine("Argument Error: " + ae.Message);

            }
            catch (Exception e)
            {
                Console.WriteLine("Error Exception: " + e);
                Console.WriteLine(e.StackTrace);
            }
            finally
            {
                Console.WriteLine("Evaluated, exiting the function.");
            }
            return strExp;
        }

        private void button5_Click(object sender, EventArgs e)//The operators buttons is used then these function displays it in the textbox
        {
            try
            {
                if (textBox2.Text == "0")
                {
                    textBox2.Clear();
                }
                Button button = (Button)sender;
                textBox2.Text = textBox2.Text + button.Text;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error Exception: " + ex);
                Console.WriteLine(ex.StackTrace);
            }
            finally
            {
                Console.WriteLine("Evaluated exiting the function.");
            }
        }

        /*
		Name: Newtons Method to find square root
		Parameters: nSquareval
		Return:	Returns the float value fx
		Author: PC	
		Description: This gives the square root of the given number.
		*/
        public float NewtonMethod(float nSquareval) //This is to find the square root of an number using newtons method
        {
            float fx = 0;
            int na = 10;
            try
            {
                if (nSquareval < 0)
                {
                    throw new ArgumentException("Input cannot be negative.");
                }
                fx = nSquareval / 2;
                for (int i = 0; i < na; i++)
                {
                    fx = (fx + nSquareval / fx) / 2;
                }
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }
            catch (Exception e)
            {
                Console.WriteLine("Something is wrong: " + e.Message);
                Console.WriteLine(e.StackTrace);
            }
            
            finally
            {
                Console.WriteLine("Evaluated exiting the function.");
            }
            return fx;
        }
        private void button21_Click(object sender, EventArgs e)
        {
            try
            {
                float fResult = float.Parse(textBox2.Text); //try catch finally
                fResult = NewtonMethod(fResult);
                textBox2.Text = fResult.ToString();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Something is wrong: " + ex.Message);
                Console.WriteLine(ex.StackTrace);
            }
            
            finally
            {
                Console.WriteLine("Evaluated exiting the function.");
            }

        }

        private void button22_Click(object sender, EventArgs e)
        {
            try
            {
                Button button = (Button)sender;
                textBox2.Text = textBox2.Text + button.Text;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Something is wrong: " + ex.Message);
                Console.WriteLine(ex.StackTrace);
            }

            finally
            {
                Console.WriteLine("Evaluated exiting the function.");
            }
        }

        /*
		Name: Decimal to Hexadecimal
		Parameters: strdecimal
		Return:	Returns the strhexdecimal 
		Author: PC	
		Description: This converts the decimal value to the hexadecimal value
		
		*/
        public string DecimalToHexadecimal(string strdecimal)
        {
            char[] chex = { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', 'A', 'B', 'C', 'D', 'E', 'F' };
            string strhexdecimal = "";
            if (string.IsNullOrEmpty(strdecimal))
            {
                throw new ArgumentException("Input cannot be null or empty.");
            }
            int ndec = int.Parse(strdecimal);

            try
            {
                if (ndec == 0)
                {
                    strhexdecimal = "0";
                }
                else
                {
                    // Convert the decimal number to hexadecimal
                    while (ndec > 0)
                    {
                        int remainder = ndec % 16;
                        strhexdecimal = chex[remainder] + strhexdecimal;
                        ndec = ndec / 16;
                    }
                }
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }
            catch (Exception e)
            {
                Console.WriteLine("Something is wrong: " + e.Message);
                Console.WriteLine(e.StackTrace);
            }
            

            finally
            {

                Console.WriteLine("Evaluated exiting the function.");
            }

            return strhexdecimal;

        }

        private void button28_Click(object sender, EventArgs e)
        {
            try
            {
                String strResult = textBox2.Text;
                strResult = DecimalToHexadecimal(strResult);
                textBox2.Text = strResult;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Something is wrong: " + ex.Message);
                Console.WriteLine(ex.StackTrace);
            }
            

            finally
            {
                Console.WriteLine("Evaluated, exiting the function.");
            }

        }

        /*
		Name: Hexadecimal to Decimal
		Parameters: strS
		Return:	Returns the nval  
		Author: PC	
		Description: This converts the Hexadecimal value to the decimal value
		
		*/
        public int HexadecimalToDecimal(string strS)
        {
            int nval = 0;
            int nd = 0;
            char c;
            string hex = "0123456789ABCDEF";
            if (string.IsNullOrEmpty(strS))
            {
                Console.WriteLine("Error: The input string cannot be null or empty.");
                return -1; // Return -1 to indicate an error
            }
            strS = strS.ToUpper();

            try
            {
                for (int i = 0; i < strS.Length; i++)
                {
                    c = strS[i]; // Get the current character

                    // Manually find the index of the character in the hex string
                    for (int j = 0; j < hex.Length; j++)
                    {
                        if (hex[j] == c)
                        {
                            nd = j;
                            break;
                        }
                    }
                    if (nd == -1)
                    {
                        throw new FormatException("Invalid character 'c' in hexadecimal string.");
                    }
                    nval = 16 * nval + nd;
                }
            }
            catch (FormatException e)
            {
                Console.WriteLine("Error: " + e.Message);
                
            }
            catch (Exception e)
            {
                Console.WriteLine("Something is wrong: " + e.Message);
                Console.WriteLine(e.StackTrace);
            }
            
            finally
            {
                Console.WriteLine("Evaluated, exiting the function.");
            }
            return nval;
        }

        private void button29_Click(object sender, EventArgs e)
        {
            try
            {
                String strS = textBox2.Text;
                int nValue = HexadecimalToDecimal(strS);
                textBox2.Text = nValue.ToString();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Something is wrong: " + ex.Message);
                Console.WriteLine(ex.StackTrace);
            }
            
            finally
            {
                Console.WriteLine("Evaluated, exiting the function.");
            }

        }

        /*
		Name: Decimal to Binary
		Parameters: nDeci
		Return:	This returns the string that is the binary number of a decimal number
		Author: PC	
		Description: This converts the decimal value to the binary value.
		
		*/
        public string DecimalToBinary(int nDeci)
        {
            int i = 0;
            if (nDeci < 0)
            {
                throw new ArgumentException("Input cannot be negative.");
            }
            if (nDeci == 0)
            {
                return "0";
            }
            int[] binary = new int[100]; // Creating an array to store the binary numbers together
            string binaryString = "";

            try
            {
                while (nDeci != 0)
                {
                    binary[i] = nDeci % 2;
                    nDeci = nDeci / 2;
                    i++;
                }

                for (int j = i - 1; j >= 0; j--)
                {
                    binaryString += binary[j].ToString();
                }
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }
            catch (Exception e)
            {
                Console.WriteLine("Something is wrong: " + e.Message);
                Console.WriteLine(e.StackTrace);
            }
            
            finally
            {
                Console.WriteLine("Evaluated exiting the function.");
            }
            return binaryString;
        }


        private void button30_Click(object sender, EventArgs e)
        {
            try
            {
                int nValue = int.Parse(textBox2.Text);
                String strS = DecimalToBinary(nValue);
                textBox2.Text = strS;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Something is wrong: " + ex.Message);
                Console.WriteLine(ex.StackTrace);
            }
            finally
            {
                Console.WriteLine("Evaluated, exiting the function.");
            }

        }

        /*
		Name: Binary to Decimal 
		Parameters: nBnum
		Return:	returns the decimal value (nDecimalValue) 
		Author: PC	
		Description: This converts the Binary value to the decimal value
		
		*/
        public int BinaryToDecimal(string nBnum)
        {
            int nDecimalValue = 0;
            int nBase = 1;
            int nThird; // Third element where we store the copy of our number
            int nRem;
           
            try
            {
                if (string.IsNullOrEmpty(nBnum) )
                {
                    throw new ArgumentException("Input must be a non-empty string containing only binary digits (0 and 1).");
                }
                nThird = Convert.ToInt32(nBnum); // Storing the number in the third variable

                while (nThird > 0)
                {
                    nRem = nThird % 10;
                    nDecimalValue += nRem * nBase;
                    nBase = nBase * 2;
                    nThird /= 10;
                }
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine("Error: " + ex.Message);
                nDecimalValue = -1; // Return -1 to indicate an error
            }
            catch (Exception e)
            {
                Console.WriteLine("Something is wrong: " + e.Message);
                Console.WriteLine(e.StackTrace);
            }
            
            finally
            {
                Console.WriteLine("Evaluated exiting the function.");
            }
            return nDecimalValue;
        }

        private void button31_Click(object sender, EventArgs e)
        {
            try
            {
                String strS = textBox2.Text;
                int nValue = BinaryToDecimal(strS);
                textBox2.Text = nValue.ToString();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Something is wrong: " + ex.Message);
                Console.WriteLine(ex.StackTrace);
            }
            finally
            {
                Console.WriteLine("Evaluated, exiting the function.");
            }

        }

        /*
		Name: Factorial 
		Parameters: BiNumber
		Return:	returns the factorial of a given number 
		Author: PC	
		Description: This gives the factorial value of the number that is been given as input
		
		*/

        public BigInteger Factorial(BigInteger BiNumber) //Todo: if the number that has a larger value wg 80! it setting the value to 0 fixit
        {
            BigInteger fact = 1;
            try
            {
                if (BiNumber < 0)
                {
                    throw new ArgumentException("Factorial is not defined for negative numbers.");
                }
                for (int i = 1; i <= BiNumber; i++)
                {
                    fact = fact * i;  //Calculates the factorial of number iterating from 1 to the nth number which is BiNumber
                }
                
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine("Error: " + ex.Message);
                // Set fact to a default value, indicating an error
                fact = 0;
            }
            catch (Exception e)
            {
                Console.WriteLine("Something is wrong: " + e.Message);
                Console.WriteLine(e.StackTrace);
            }
            finally
            {
                Console.WriteLine("Evaluated exiting the function.");
            }
            return fact;
        }

        private void button32_Click(object sender, EventArgs e)
        {
            try
            {
                BigInteger BiNum = BigInteger.Parse(textBox2.Text);
                BiNum = Factorial(BiNum);
                textBox2.Text = BiNum.ToString();
            }  
            catch (Exception ex)
            {
                Console.WriteLine("Something is wrong: " + ex.Message);
                Console.WriteLine(ex.StackTrace);
            }
          
            finally
            {
                Console.WriteLine("Evaluated exiting the function.");
            }
        }
    }
}


/*
ArgumentOutOfRangeException:
This occurs if nOpenIndex + 1 or nCloseIndex - nOpenIndex - 1 results in indices that are out of bounds for the string. For example, if nCloseIndex is less than or equal to nOpenIndex, the length provided for the substring could be negative or zero, leading to this exception.

ArgumentNullException:
If strExpression is null, attempting to call Substring on it will throw this exception.


General Exception:
Any other unforeseen errors related to string manipulation might occur, and handling them is important to ensure robustness.
 
 */
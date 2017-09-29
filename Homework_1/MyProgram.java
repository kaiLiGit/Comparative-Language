/**
 *  Interpreter Assignment Comparative Programming Language
 *  This java file is written entirely by me and is the first version.
 *  Author: Kai Li
 *  Date: September 3, 2017
 *  That said, there are more functionalities to be added to the interpretor
 *  and more thorough code inspection has to be carried out to pinpoint 
 *  the pivotal errors. Even though the program met all of the homework 
 *  requirements but it does not indicate this program is error-free. 
 *  Many more features can be added to this very basic interpreter. 
 *  Disclaimer: any person attempted to copy-paste this Java file is 
 *              subject to total responsibility of academic policy 
 *              at their own institutions. This is only available for 
 *              viewing but not referencing and citing. This file should 
 *              remain as is at all times and is not available for sales 
 *              or reuse since I myself will NOT be responsible for any 
 *              demage incurred through using such program. 
 * */
import java.io.BufferedReader;
import java.io.FileReader;
import java.io.IOException;
import java.util.Arrays;
import java.util.Hashtable;

public class MyProgram {
	private static String fileName;
	private static Hashtable<String, String> stringTable;

	private static final String LETTER_REGEX = "[a-zA-Z]+";
	private static final String INT_REGEX = "-?[1-9]\\d*|0";
	private static final String SPACE_REGEX = "\\s+";
	private static final String EMPTY = Integer.MIN_VALUE + "";

	// private static Hashtable<String, Integer> intTable;

	public static Hashtable<String, String> getStringTable() {
		if (stringTable == null) {
			stringTable = new Hashtable<String, String>();
		}
		return stringTable;
	}

	/*
	 * public static Hashtable<String, Integer> getIntTable() { if(intTable ==
	 * null) { intTable = new Hashtable<String, Integer>(); } return intTable; }
	 */

	public static void equalSign(int index, String tokens[], int lineNumber) throws Exception { // '=',
		// no
		// arithmetic
		if (tokens == null) {
			return;
		}
		String LHS;
		String RHS;
		String lhsVal;
		String rhsVal;
		if ((index - 1) >= 0 && (index + 1) <= tokens.length - 1) {
			LHS = tokens[index - 1];
			RHS = tokens[index + 1];
			lhsVal = stringTable.get(LHS);
			rhsVal = stringTable.get(RHS);

			if (lhsVal != null && rhsVal == null) { // var = literal
				if (!RHS.matches(INT_REGEX)) { // string literal, quotes trimmed
					RHS = RHS.substring(1, RHS.length() - 1);
				}
				stringTable.put(LHS, RHS);
			}

			if (lhsVal != null && rhsVal != null) { // both are variables
				if (!(stringTable.get(LHS).equals(EMPTY)) && !(stringTable.get(RHS).equals(EMPTY))) {
					stringTable.put(LHS, stringTable.get(RHS));
				} else { // variables had not been used, show ERROR
					// System.out.println("RUNTIME ERROR: " + lineNumber);
					throw new Exception("RUNTIME ERROR: line " + lineNumber);
				}
			}
		}
	}

	public static void plusEqualSign(int index, String tokens[], int lineNumber) throws Exception {
		if (tokens == null) {
			return;
		}
		String LHS;
		String RHS;
		String lhsVal;
		String rhsVal;

		if ((index - 1) >= 0 && (index + 1) <= tokens.length - 1) {
			LHS = tokens[index - 1];
			RHS = tokens[index + 1];
			lhsVal = stringTable.get(LHS);
			rhsVal = stringTable.get(RHS);

			if (lhsVal != null && rhsVal == null) { // var += literal value
				// Only two data type (String and Integer), so checking Right
				// hand side "not" Integer is sufficient
				if (lhsVal.matches(LETTER_REGEX) && (!RHS.matches(INT_REGEX))) { // String
																					// +=
																					// String
																					// Literal
					RHS = RHS.substring(1, RHS.length() - 1); // get rid of
																// quotes
					stringTable.put(LHS, lhsVal + RHS);
				} else if (lhsVal.matches(INT_REGEX) && (RHS.matches(INT_REGEX))) { // Int
																					// var
																					// +=
																					// Int
																					// literal
					stringTable.put(LHS, (Integer.parseInt(lhsVal) + Integer.parseInt(RHS)) + "");
				} else { // other combination, Error
					// System.out.println("RUNTIME ERROR: " + lineNumber);
					throw new Exception("RUNTIME ERROR: line " + lineNumber);
				}
			}

			if (lhsVal != null && rhsVal != null) { // var += var
				// check if values assign to variables or variables had been
				// used before assignment
				if (!(stringTable.get(LHS).equals(EMPTY)) && !(stringTable.get(RHS).equals(EMPTY))) {
					if (!(lhsVal.matches(INT_REGEX)) && !(rhsVal.matches(INT_REGEX))) { // String
																						// var
																						// +=
																						// String
																						// var
						// lhsVal = lhsVal.substring(1, lhsVal.length() - 1); //
						// get rid of quotes
						// rhsVal = rhsVal.substring(1, rhsVal.length() - 1); //
						// get rid of quotes
						stringTable.put(LHS, lhsVal + rhsVal);
					} else if (lhsVal.matches(INT_REGEX) && rhsVal.matches(INT_REGEX)) { // int
																							// var
																							// +=
																							// int
																							// var
						stringTable.put(LHS, Integer.parseInt(lhsVal) + Integer.parseInt(rhsVal) + "");
					} else { // other combinations, show ERROR
						// System.out.println("RUNTIME ERROR: " + lineNumber);
						throw new Exception("RUNTIME ERROR: line " + lineNumber);
					}
				} else { // variables had not been used, show ERROR
					// System.out.println("RUNTIME ERROR: " + lineNumber);
					throw new Exception("RUNTIME ERROR: line " + lineNumber);
				}
			}
		}

	}

	public static void minusEqual(int index, String[] tokens, int lineNumber) throws Exception {
		if (tokens == null) {
			return;
		}
		String LHS;
		String RHS;
		String lhsVal;
		String rhsVal;

		if ((index - 1) >= 0 && (index + 1) <= tokens.length - 1) {
			LHS = tokens[index - 1];
			RHS = tokens[index + 1];
			lhsVal = stringTable.get(LHS);
			rhsVal = stringTable.get(RHS);
			if (lhsVal != null && rhsVal == null) { // var -= literal value
				if (lhsVal.matches(INT_REGEX) && RHS.matches(INT_REGEX)) { // int
																			// -=
																			// int
																			// literal
					stringTable.put(LHS, (Integer.parseInt(lhsVal) - Integer.parseInt(RHS)) + "");
				} else { // other combinations, show ERROR
					// System.out.println("RUNTIME ERROR: " + lineNumber);
					throw new Exception("RUNTIME ERROR: line " + lineNumber);
				}
			}

			if (lhsVal != null && rhsVal != null) { // var -= var
				// check if two variables were used
				if (!(stringTable.get(LHS).equals(EMPTY)) && !(stringTable.get(RHS).equals(EMPTY))) {
					if (lhsVal.matches(INT_REGEX) && rhsVal.matches(INT_REGEX)) { // int
																					// variable
																					// -=
																					// int
																					// variable
						stringTable.put(LHS, (Integer.parseInt(lhsVal) - Integer.parseInt(rhsVal)) + "");
					} else {
						// System.out.println("RUNTIME ERROR: " + lineNumber);
						throw new Exception("RUNTIME ERROR: line " + lineNumber);
					}
				} else { // one or two of the LHS/RHS was not given value
					// System.out.println("RUNTIME ERROR: " + lineNumber);
					throw new Exception("RUNTIME ERROR: line " + lineNumber);
				}
			}
		}
	}

	public static void multiplyEqual(int index, String[] tokens, int lineNumber) throws Exception {
		if (tokens == null) {
			return;
		}
		String LHS;
		String RHS;
		String lhsVal;
		String rhsVal;

		if ((index - 1) >= 0 && (index + 1) <= tokens.length - 1) {
			LHS = tokens[index - 1];
			RHS = tokens[index + 1];
			lhsVal = stringTable.get(LHS);
			rhsVal = stringTable.get(RHS);

			if (lhsVal != null && rhsVal == null) { // var *= literal value
				if (lhsVal.matches(INT_REGEX) && RHS.matches(INT_REGEX)) {
					stringTable.put(LHS, (Integer.parseInt(lhsVal) * Integer.parseInt(RHS)) + "");
				} else {
					// System.out.println("RUNTIME ERROR: " + lineNumber);
					throw new Exception("RUNTIME ERROR: line " + lineNumber);
				}
			}

			if (lhsVal != null && rhsVal != null) { // var *= var
				if (!(stringTable.get(LHS).equals(EMPTY)) && !(stringTable.get(RHS).equals(EMPTY))) {
					if (lhsVal.matches(INT_REGEX) && rhsVal.matches(INT_REGEX)) { // int
																					// variable
																					// *=
																					// int
																					// variable
						stringTable.put(LHS, (Integer.parseInt(lhsVal) * Integer.parseInt(rhsVal)) + "");
					} else {
						// System.out.println("RUNTIME ERROR: " + lineNumber);
						throw new Exception("RUNTIME ERROR: line " + lineNumber);
					}
				} else { // show ERROR if variables had not been used before
							// assignment
					// System.out.println("RUNTIME ERROR: " + lineNumber);
					throw new Exception("RUNTIME ERROR: line " + lineNumber);
				}
			}
		}
	}

	public static void print(int index, String tokens[]) {
		if ((index + 1) <= tokens.length - 1) {
			String key = tokens[index + 1];
			String value = stringTable.get(key);
			if (value != null) {
				System.out.printf("%s = %s\n", key, value);
			}
		}
	}

	private static final int OPERATOR_INDEX = 1;

	private static boolean forloopProcess(int i, String tokens[], int lineNumber) throws Exception {
		// extract iteration value position, 1 position to the right of 'FOR'
		int iter = Integer.parseInt(tokens[i + 1]);
		// process statement within a for-loop, starting from index 2 of
		// tokens array
		tokens = Arrays.copyOfRange(tokens, 2, tokens.length - 1);
		int start = 0; // start, index for sub-tokens extractions
		String subTokens[];
		int it = 0; // iteration counter
		int endforIndex = 0; // index for tracking end for position

		for (it = 0; it < iter; it++) {
			for (int k = 0; k < tokens.length; k++) { // iterating through the
														// tokens array
				if (tokens[k].equals(";")) {
					subTokens = Arrays.copyOfRange(tokens, start, k);
					start = k + 1;
					processLine(OPERATOR_INDEX, subTokens, lineNumber);
				}

				if (tokens[k].equals("FOR")) {
					// get ENDFOR index
					for (int f = k + 1; f < tokens.length; f++) {
						if (tokens[f].equals("FOR")) {
							for (endforIndex = tokens.length - 1; endforIndex >= 0; endforIndex--) {
								if (tokens[endforIndex].equals("ENDFOR")) {
									break;
								}
							}
							break;
						}
						if (tokens[f].equals("ENDFOR")) {
							endforIndex = f;
							break;
						}
					}

					// extract sub-array of nested FOR-ENDFOR
					subTokens = Arrays.copyOfRange(tokens, start, endforIndex + 1);
					processLine(0, subTokens, lineNumber);

					k = endforIndex;
					start = k + 1;
				}
			}

			// clear indices values after every iteration
			start = 0;
			endforIndex = 0;
		}

		if (it == iter) {
			return true;
		}
		return false;
	}

	public static boolean processLine(int i, String[] tokens, int lineNumber) throws Exception {

		// System.out.println(Arrays.toString(tokens));

		String token;
		token = tokens[i];
		switch (token) {
		case "=":
			equalSign(i, tokens, lineNumber);
			break;
		case "+=":
			plusEqualSign(i, tokens, lineNumber);
			break;
		case "-=":
			minusEqual(i, tokens, lineNumber);
			break;
		case "*=":
			multiplyEqual(i, tokens, lineNumber);
			break;
		case "PRINT":
			print(i, tokens);
			break;
		case "FOR":
			return forloopProcess(i, tokens, lineNumber);
		default: // Variables and Literal values
			if (token.matches(LETTER_REGEX)) {
				if (stringTable.get(token) == null) {
					stringTable.put(token, EMPTY);
				}
			}
			break;
		}

		return false;
	}

	public static void main(String[] args) {
		fileName = args[0];
		System.out.println(Arrays.toString(args));

		stringTable = getStringTable();
		// intTable = getIntTable();

		BufferedReader br = null;
		FileReader fr = null;

		try {
			fr = new FileReader(fileName);
			br = new BufferedReader(fr);

			String currentLine;
			int lineNumber = 0;

			while ((currentLine = br.readLine()) != null) {
				// System.out.println(currentLine);
				try {
					String tokens[] = currentLine.split(SPACE_REGEX);
					for (int i = 0; i < tokens.length; i++) {
						if (processLine(i, tokens, ++lineNumber))
							break;
					}
				} catch (Exception e) {
					System.out.println(e.getMessage());
					break;
				}
			}

			// System.out.println("hashT: " + stringTable.toString());

		} catch (IOException e) {
			e.printStackTrace();
		} finally {
			try {
				if (br != null) {
					br.close();
				}

				if (fr != null) {
					fr.close();
				}
			} catch (IOException ex) {
				ex.printStackTrace();
			}
		}
	}
}

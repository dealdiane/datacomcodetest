import { useState } from "react";
import * as Yup from 'yup';
import { Formik, Field, Form, ErrorMessage } from 'formik';
import { readString, jsonToCSV } from "react-papaparse";

export default function PayslipCsvForm({ onGenerated }) {
    const [outputCsv, setOutputCsv] = useState('');

    const payslipSchema = Yup.object().shape({
        csv: Yup.string()
            .required('CSV field is required')
    });

    const convertMonthNameToPeriod = (monthName) => new Date(Date.parse(monthName + " 1, 1975")).getMonth() + 1;

    const createPayslipParameterFromCsvRow = (csvRow) => {
        if (csvRow.length === 5) {
            const payslipParameter = {
                firstName: csvRow[0],
                lastName: csvRow[1],
            };

            const annualSalary = parseFloat(csvRow[2]);

            if (isNaN(annualSalary) || superannuationRate < 0) {
                throw 'Invalid annual salary';
            }

            payslipParameter.annualSalary = annualSalary;

            const monthName = csvRow[4];

            try {
                payslipParameter.paymentPeriod = convertMonthNameToPeriod(monthName);
            } catch {
                throw 'Invalid month';
            }

            const superannuationRate = parseFloat(csvRow[3].match(/([\.\d]+)%/i)?.at(1));

            if (isNaN(superannuationRate) || superannuationRate < 0 || superannuationRate > 100) {
                throw 'Invalid superannuation rate';
            }

            payslipParameter.superannuationRate = superannuationRate;

            return payslipParameter;
        }

        if (csvRow.join('').length) {
            throw 'Invalid data';
        }

        return null;
    };

    const createPayslipParameters = (csv) => {
        const { data, errors: readErrors } = readString(csv, {
            delimiter: ',',
            skipEmptyLines: false // Skipping empty lines may cause incorrect line number error messages
        });

        const errors = readErrors.map(({ message, row }) =>
            isNaN(row)
                ? message
                : `${message} at line ${row + 1}`);

        const payslipParameters = data.reduce((values, item, lineNumber) => {
            try {
                const payslipParameter = createPayslipParameterFromCsvRow(item);

                if (payslipParameter) {
                    values.parameters.push(payslipParameter);
                }
            } catch (errorMessage) {
                values.errors.push(`Error '${errorMessage}' at line ${lineNumber + 1}`);
            }

            return values;
        }, { parameters: [], errors: errors });

        return payslipParameters;
    };

    const setCsvOutputFromResponse = async (response) => {
        // TODO: Handle exception if response format is unexpected
        const payslips = await response.json();
        const payslipRows = payslips.map(payslip => {
            return {
                name: payslip.name,
                payPeriod: `${payslip.periodStart} - ${payslip.periodEnd}`,
                grossIncome: payslip.grossIncomeAmount.toFixed(2),
                tax: payslip.taxAmount.toFixed(2),
                netIncome: payslip.netIncomeAmount.toFixed(2),
                superannuation: payslip.superannuationAmount.toFixed(2),
            };
        });

        const csv = jsonToCSV(payslipRows, {
            header: false,
        });

        setOutputCsv(csv);
    }

    const getNormalizedErrorsFromResponse = async function* (response) {
        let serverErrors;

        try {
            serverErrors = await response.json();
        } catch {
            yield 'A server error has occurred. Please try again.';
            return;
        }

        for (const property in serverErrors) {
            const nameMatch = property.match(/\[(\d+)\]\.(.+)/);
            const errorMessage = serverErrors[property];

            if (nameMatch.length === 3) {
                const rowNumber = parseInt(nameMatch[1]);
                yield `Error at line ${rowNumber + 1}: ${errorMessage}`;
            } else {
                yield errorMessage;
            }
        }
    }

    const submitForm = (values, { setErrors, setSubmitting }) => {
        !(async () => {
            const { parameters, errors } = createPayslipParameters(values.csv);

            try {
                if (!errors.length) {
                    const rawResponse = await fetch('/api/payslip/generatelist', {
                        method: 'POST',
                        headers: {
                            'Accept': 'application/json',
                            'Content-Type': 'application/json'
                        },
                        body: JSON.stringify(parameters)
                    });

                    if (rawResponse.ok) {
                        await setCsvOutputFromResponse(rawResponse);
                    } else {
                        const serverErrors = getNormalizedErrorsFromResponse(rawResponse);

                        for await (const serverError of serverErrors) {
                            errors.push(serverError);
                        }
                    }
                }

                if (errors.length) {
                    setErrors({
                        csv: errors.join('\n')
                    });
                }
            } finally {
                setSubmitting(false);
            }
        })();
    };

    return (
        <>
            <div className="card">
                <div className="card-body">
                    <div className="card-title">
                        <span className="h6">Sample input CSV:</span>
                    </div>
                    <pre>{`John,Smith,60050,9%,March
Alex,Wong,120000,10%,March`}
                    </pre>
                </div>
            </div>
            <Formik
                initialValues={{
                    csv: ''
                }}
                validationSchema={payslipSchema}
                onSubmit={submitForm}
            >
                {(formik) => (
                    <Form className="form-csv">
                        <div className="mb-3 mt-5">
                            <label className="form-label" htmlFor="firstName">CSV:</label>
                            <Field component="textarea" className="form-control" id="csv" name="csv"></Field>
                            <span className="invalid-feedback text-pre">
                                <ErrorMessage name="csv" />
                            </span>
                        </div>
                        <div>
                            <button className="btn btn-primary" type="submit" disabled={formik.isSubmitting}>Generate Payslip CSV
                                {!!formik.isSubmitting && <small className="spinner-border spinner-border-sm ms-2"></small>}
                            </button>
                        </div>
                    </Form>
                )}
            </Formik>
            {!!outputCsv.length &&
                <div className="card mt-5">
                    <div className="card-body">
                        <div className="card-title">
                            <span className="h6">Output CSV:</span>
                        </div>
                        <pre>{outputCsv}</pre>
                    </div>
                </div>}
        </>
    );
}
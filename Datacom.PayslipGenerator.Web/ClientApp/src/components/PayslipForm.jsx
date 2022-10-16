import { useState } from "react";
import { Formik, Field, Form, ErrorMessage } from 'formik';
import * as Yup from 'yup';
import MonthOptions from './MonthOptions'

export default function PayslipForm({ onGenerated }) {
    const minNameLength = 2;
    const maxNameLength = 50;

    const [serverError, setServerError] = useState('');

    const payslipSchema = Yup.object().shape({
        firstName: Yup.string()
            .min(minNameLength, `First name must be at least ${minNameLength} characters.`)
            .max(maxNameLength, `First name must not be longer than ${maxNameLength} characters.`),
        lastName: Yup.string()
            .min(minNameLength, `Last name must be at least ${minNameLength} characters.`)
            .max(maxNameLength, `last name must not be longer than ${maxNameLength} characters.`),
        annualSalary: Yup.number()
            .required('Annual salary is required.')
            .positive('Annual salary must be a positive number.')
            .max(100_000_000, 'Annual salary cannot be more than $100,000,000.')
            .integer('Annual salary must be a number.'),
        paymentPeriod: Yup.number()
            .required('Payment period is required.')
            .min(1, 'Payment period must be a number from 1 and 12.')
            .max(12, 'Payment period must be a number from 1 and 12.')
            .integer('Payment period must be a number.'),
        superannuationRate: Yup.number()
            .required('Superannuation rate is required.')
            .min(0, 'Superannuation rate must be a number between 0 and 100.')
            .max(100, 'Superannuation rate must be a number between 0 and 100.')
    });

    const submitForm = (values, { setErrors, setSubmitting }) => {
        setServerError('');

        const requestValues = { ...values };

        // Remove name properties if an empty string
        // to stop string length server side validation from triggering.
        // TODO: Use FluentValidation so empty strings are propertly supported from both here and in the CSV form.
        if (!requestValues.firstName) {
            delete requestValues.firstName;
        }

        if (!requestValues.lastName) {
            delete requestValues.lastName;
        }

        !(async () => {
            try {
                const rawResponse = await fetch('/api/payslip/generatesingle', {
                    method: 'POST',
                    headers: {
                        'Accept': 'application/json',
                        'Content-Type': 'application/json'
                    },
                    body: JSON.stringify(requestValues)
                });

                let jsonResponse;

                try {
                    jsonResponse = await rawResponse.json();
                } catch {
                    setServerError('A server error has occurred. Please try again.');
                }

                if (rawResponse.ok) {
                    onGenerated(jsonResponse);
                } else {
                    const error = {};

                    for (const serverPropertyName in jsonResponse) {
                        // Convert property name to camelCase.
                        // Assumes that server properties are PascalCased.
                        const propertyName = serverPropertyName.charAt(0).toLowerCase() + serverPropertyName.slice(1)
                        error[propertyName] = jsonResponse[serverPropertyName].join(';');
                    }

                    setErrors(error);
                }
            } finally {
                setSubmitting(false);
            }
        })();
    };

    return (
        <Formik
            initialValues={{
                firstName: '',
                lastName: '',
                annualSalary: 44000,
                superannuationRate: 10,
                paymentPeriod: new Date().getMonth()
            }}
            validationSchema={payslipSchema}
            onSubmit={submitForm}
        >
            {(formik) => (
                <Form>
                    {serverError && serverError.length && <div className="invalid-feedback">{serverError}</div>}
                    <div className="mb-3">
                        <label className="form-label" htmlFor="firstName">First Name:</label>
                        <Field className="form-control" id="firstName" name="firstName"></Field>
                        <span className="invalid-feedback">
                            <ErrorMessage name="firstName" />
                        </span>
                    </div>
                    <div className="mb-3">
                        <label className="form-label" htmlFor="lastName">Last Name:</label>
                        <Field className="form-control" id="lastName" name="lastName"></Field>
                        <span className="invalid-feedback">
                            <ErrorMessage name="lastName" />
                        </span>
                    </div>
                    <div className="mb-3">
                        <label className="form-label" htmlFor="annualSalary">Annual Salary:</label>
                        <div className="input-group mb-3">
                            <span className="input-group-text">$</span>
                            <Field className="form-control" id="annualSalary" name="annualSalary" type="number"></Field>
                        </div>
                        <span className="invalid-feedback">
                            <ErrorMessage name="annualSalary" />
                        </span>
                    </div>
                    <div className="mb-3">
                        <label className="form-label" htmlFor="paymentPeriod">Period:</label>
                        <Field
                            className="form-control"
                            id="paymentPeriod"
                            name="paymentPeriod"
                            component="select"
                        >
                            <MonthOptions />
                        </Field>
                        <span className="invalid-feedback">
                            <ErrorMessage name="paymentPeriod" />
                        </span>
                    </div>
                    <div className="mb-3">
                        <label className="form-label" htmlFor="superannuationRate">Superannuation Rate:</label>
                        <div className="input-group mb-3">
                            <Field className="form-control" id="superannuationRate" name="superannuationRate" type="number"></Field>
                            <span className="input-group-text">%</span>
                        </div>
                        <span className="invalid-feedback">
                            <ErrorMessage name="superannuationRate" />
                        </span>
                    </div>
                    <div>
                        <button className="btn btn-primary" type="submit" disabled={formik.isSubmitting}>Generate Payslip
                            {!!formik.isSubmitting && <small className="spinner-border spinner-border-sm ms-2"></small>}
                        </button>
                    </div>
                </Form>
            )}

        </Formik>
    );
}
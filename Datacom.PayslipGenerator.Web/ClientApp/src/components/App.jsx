import { useState, useEffect } from "react";
import {
    createBrowserRouter,
    RouterProvider
} from "react-router-dom";

import PayslipForm from "./PayslipForm";
import PayslipDetail from "./PayslipDetail";
import PayslipCsvForm from "./PayslipCsvForm";

export default function App() {
    const [payslip, setPayslip] = useState();

    const onGenerated = (generatedPayslip) => {
        setPayslip({
            grossIncome: generatedPayslip.grossIncomeFormattedAmount,
            netIncome: generatedPayslip.netIncomeFormattedAmount,
            periodEnd: generatedPayslip.periodEnd,
            periodStart: generatedPayslip.periodStart,
            superannuation: generatedPayslip.superannuationFormattedAmount,
            tax: generatedPayslip.taxFormattedAmount,
            name: generatedPayslip.name,
        });
    }

    const router = createBrowserRouter([
        {
            element: (
                <>
                    <header className="pb-3 mb-5">
                        <ul className="nav nav-tabs">
                            <li className="nav-item">
                                <a className="nav-link active" aria-current="page" href="/app/">Form Fields</a>
                            </li>
                            <li className="nav-item">
                                <a className="nav-link" aria-current="page" href="/app/csv">CSV</a>
                            </li>
                        </ul>
                    </header>
                    <PayslipForm onGenerated={onGenerated} />
                    {!!payslip &&
                        <section className="mt-5">
                            <div className="card">
                                <div className="card-body">
                                    <PayslipDetail
                                        grossIncome={payslip.grossIncome}
                                        netIncome={payslip.netIncome}
                                        superannuation={payslip.superannuation}
                                        tax={payslip.tax}
                                        periodStart={payslip.periodStart}
                                        periodEnd={payslip.periodEnd}
                                        name={payslip.name}
                                    />
                                </div>
                            </div>
                        </section>}
                </>),
            path: "/app"
        },
        {
            element: (
                <>
                    <header className="pb-3 mb-5">
                        <ul className="nav nav-tabs">
                            <li className="nav-item">
                                <a className="nav-link" aria-current="page" href="/app/">Form Fields</a>
                            </li>
                            <li className="nav-item">
                                <a className="nav-link active" aria-current="page" href="/app/csv">CSV</a>
                            </li>
                        </ul>
                    </header>
                    <PayslipCsvForm />
                </>
            ),
            path: "/app/csv",
        }
    ]);

    return (
        <RouterProvider router={router} />
    );
}
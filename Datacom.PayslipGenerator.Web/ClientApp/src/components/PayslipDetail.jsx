export default function PayslipDetail({
    name,
    grossIncome,
    netIncome,
    superannuation,
    tax,
    periodStart,
    periodEnd
}) {

    return (
        <>
            <h4>Payslip for period {periodStart} - {periodEnd}</h4>
            <table className="table mt-4">
                <tbody>
                    <tr>
                        <td>Name</td>
                        <td>
                            <span className="fw-bold">{name}</span>
                        </td>
                    </tr>
                    <tr>
                        <td>Gross Income</td>
                        <td>{grossIncome}</td>
                    </tr>
                    <tr>
                        <td>Income Tax</td>
                        <td>
                            <span className="text-danger">({tax})</span>
                        </td>
                    </tr>
                    <tr>
                        <td>Net Income</td>
                        <td>
                            <span className="text-success fw-bold">{netIncome}</span>
                        </td>
                    </tr>
                    <tr>
                        <td>Superannuation</td>
                        <td>{superannuation}</td>
                    </tr>
                </tbody>

            </table>
        </>
    );
}
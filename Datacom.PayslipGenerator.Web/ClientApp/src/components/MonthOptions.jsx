export default function MonthOptions() {
    return <>
        {
            [...Array(12).keys()].map(month =>
                <option key={month} value={month + 1}>{(new Date(1975, month, 1)).toLocaleString('default', { month: 'long' })}</option>
            )
        }
    </>;
}
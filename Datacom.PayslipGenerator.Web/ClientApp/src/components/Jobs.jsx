import { useState, useEffect } from "react";
import JobRow from "./JobRow";
import Moment from 'react-moment';

export default function Jobs({ jobsStartDate, jobsEndDate }) {

    const [jobs, setJobs] = useState({});
    const [regions, setRegions] = useState([]);
    const [selectedRegion, setSelectedRegion] = useState('');

    const formatDate = (date) => `${date.getFullYear()}-${(date.getMonth() + 1)}-${date.getDate()}`;
    const jobDates = [];

    const from = new Date(jobsStartDate.getTime());

    while (from <= jobsEndDate) {
        jobDates.push(parseInt(from.getTime() / 1000));
        from.setDate(from.getDate() + 1);
    }

    useEffect(() => {
        !(async function fetchData() {

            const from = formatDate(jobsStartDate);
            const to = formatDate(jobsEndDate);
            const response = await fetch(`/Umbraco/Api/Job/GetAllJobs?from=${from}&to=${to}&region=${encodeURIComponent(selectedRegion)}`);
            const jobsResponse = await response.json();

            const jobsByPhotographer = jobsResponse.reduce((photgrapherJobGroups, job) => {

                const photographerJobGroup = (photgrapherJobGroups[job.photographer] || []);

                // Ensure that dates are consistent (no time component)
                const jobDate = new Date(job.date * 1000);

                jobDate.setHours(0, 0, 0);

                photographerJobGroup.push({ ...job, date: jobDate.getTime() / 1000 });
                photgrapherJobGroups[job.photographer] = photographerJobGroup;

                return photgrapherJobGroups;
            }, {});

            for (const photographer in jobsByPhotographer) {

                const photographerJobs = jobsByPhotographer[photographer];
                const jobsByDate = photographerJobs.reduce((jobGroups, job) => {

                    const jobGroup = (jobGroups[job.date] || []);

                    jobGroup.push(job);
                    jobGroups[job.date] = jobGroup;

                    return jobGroups;
                }, {});

                for (const date of jobDates) {
                    if (!jobsByDate[date]) {
                        jobsByDate[date] = []
                    }
                }

                jobsByPhotographer[photographer] = jobsByDate;
            }

            setJobs(_ => jobsByPhotographer);
        })();
    }, [jobsStartDate?.valueOf() || 0, jobsEndDate?.valueOf() || 0, selectedRegion]);


    useEffect(() => {
        !(async function () {
            const response = await fetch('/Umbraco/Api/Region/All');
            const regions = await response.json();

            setRegions(_ => regions.sort());
        })();
    }, []);

    const renderJobRows = () => {
        return Object.keys(jobs).sort().map(photographer => {

            const photographerJobs = jobs[photographer];

            return <JobRow key={photographer} photographer={photographer} jobs={photographerJobs} />;
        });
    }

    return (
        <section>
            <div>Jobs</div>
            <select defaultValue="{selectedRegion}" onChange={event => setSelectedRegion(event.target.value)}>
                {regions.map(region => <option key={region}>{region}</option>)}
            </select>
            <table className="table">
                <thead>
                    <tr>
                        <th>Photographer</th>
                        {jobDates.map(date => {
                            return <th key={date}>
                                <Moment format="ddd" date={new Date(date * 1000)} />
                            </th>;
                        }
                        )}
                    </tr>
                </thead>
                <tbody>
                    {renderJobRows()}
                </tbody>
            </table>
        </section>
    );
}
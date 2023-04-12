import { useState, useEffect } from "react";
import { Link } from "react-router-dom";
import { getArchives } from "../../Services/ArchiveWidget";

const Archives = () => {
  const [monthList, setMonthlyList] = useState([]);

  useEffect(() => {
    getArchives().then((data) => {
      if (data) {
        setMonthlyList(data);
      } else {
        setMonthlyList([]);
      }
    });
  }, []);

  return (
    <div className="mb-4">
      <h3 className="text-success mb-2">Archives</h3>

      <div className="list-group list-group-flush">
        {monthList.map((item, index) => (
          <Link key={index} className="list-group-item d-flex align-items-start justify-content-between" to={`/archives/${item.year}/${item.month}`}>
            <div className="me-auto">
              {item.monthName} {item.year}
            </div>
            <span className="badge bg-success rounded-pill">
              {item.postCount}
            </span>
          </Link>
        ))}
      </div>
    </div>
  );
};


export default Archives;
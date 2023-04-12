import { useState, useEffect } from "react";
import { Link } from "react-router-dom";
import { getCategories } from "../Services/CategoriesWidgets";

const CategoriesWidget = (categoryItem) => {
  const [categoryList, setCategoryList] = useState([]);
  
  useEffect(() => {
    getCategories().then((data) => {
      if (data) {
        setCategoryList(data);
      } else {
        setCategoryList([]);
      }
    });
  }, []);

  return (
    <div className="mb-3">
      <h3 className="text-success mb-2">Các chủ đề</h3>

      <div className="list-group list-group-flush">
        {categoryList.map((item, index) => (
          <Link
            key={index}
            className="list-group-item d-flex align-items-start justify-content-between"
            to={`/category/${item.urlSlug}`}
          >
            <div className="me-auto">{item.name}</div>
            <span className="badge bg-success rounded-pill">
              {item.postCount}
            </span>
          </Link>
        ))}
      </div>
    </div>
  );
};

export default CategoriesWidget;

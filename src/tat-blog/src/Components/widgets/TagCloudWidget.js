import { useState, useEffect } from "react";
import { Link } from "react-router-dom";
import { getTagClould } from "../../Services/TagCloudWidget";

const TagCloudWidget = (tagItem) => {
  const [tagList, setTagCloudList] = useState([]);

  useEffect(() => {
    getTagClould().then((data) => {
      if (data) setTagCloudList(data);
      else setTagCloudList([]);
    });
  }, []);

  return (
    <div className="mb-3">
      <h3 className="text-success">Tag Cloud</h3>
      {tagList.map((item, index) => {
        return (
          <Link
            key={index}
            className="btn btn-outline-success btn-sm me-2 mb-2"
            to={`/tag/${item.urlSlug}`}
            title="Xem chi tiết"
          >
            {item.name}
            <span>&nbsp;({item.postCount})</span>
          </Link>
        );
      })}
    </div>
  );
};

export default TagCloudWidget;

import { useState, useEffect } from "react";
import ListGroup from "react-bootstrap/ListGroup";
import { Link } from "react-router-dom";
import { getTopAuthor } from "../Services/TopAuthorWidget";

const TopAuthorWidget = (topAuthor) => {
  const [topAuthorList, setAuthorList] = useState([]);
  useEffect(() => {
    getTopAuthor().then((data) => {
      if (data) {
        setAuthorList(data);
      } else {
        setAuthorList([]);
      }
    });
  });

  return (
    <div className="mb-3">
      <h3 className="text-success">Top 4 tác giả nhiều bài viết</h3>
      {topAuthorList.length > 0 && (
        <ListGroup>
          {topAuthorList.map((item, index) => {
            return (
              <ListGroup.Item key={index}>
                <Link
                  to={`/author/${item.urlSlug}`}
                  title="Xem chi tiết"
                  className="text-decoration-none"
                >
                  {item.fullName}
                  <span>&nbsp;({item.postCount})</span>
                </Link>
              </ListGroup.Item>
            );
          })}
        </ListGroup>
      )}
    </div>
  );
};

export default TopAuthorWidget;

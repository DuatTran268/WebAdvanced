import { useState, useEffect } from "react";
import  ListGroup  from "react-bootstrap/ListGroup";
import { Link } from "react-router-dom";
import { getRandomPost } from "../Services/RandomPostWidget";

const RandomPostWidget = (randomItem) => {
  const [randomPostList, setRandomPostList] = useState([]);

  useEffect(() => {
    getRandomPost().then((data) => {
      if (data) {
        setRandomPostList(data);
      } else {
        setRandomPostList([]);
      }
    });
  }, []);

  return (
    <div className="mb-3">
      <h3 className="text-success mb-2">5 bài viết ngẫu nhiên</h3>
      {randomPostList.length > 0 && 
          <ListGroup>
            {randomPostList.map((item, index) => {
              return (
                <ListGroup.Item key={index}>
                <Link
                  to={`/post/${item.urlSlug}`}
                  title="Xem chi tiết"
                  className="text-decoration-none"
                >
                  {item.title}
                </Link>

                </ListGroup.Item>
              );
            })}
          </ListGroup>
        }
    </div>
  );
};


export default RandomPostWidget;
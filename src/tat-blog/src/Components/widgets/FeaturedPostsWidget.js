import { useState, useEffect } from "react";
import ListGroup from "react-bootstrap/ListGroup";
import { Link } from "react-router-dom";
import { getFeaturedPost } from "../../Services/FeaturedPostsWidget";


const FeaturedWidget = (featuredItem) => {
  const [featuredList, setFeaturedList] = useState([]);
  useEffect(() => {
    getFeaturedPost().then(data => {
      if(data){
        setFeaturedList(data);
      }
      else{
        setFeaturedList([]);
      }
    });
  }, [])


  return (
    <div className="mb-4">
        <h3 className="text-success mb-2">
          Top 3 bài nhiều lượt xem
        </h3>

        {featuredList.length > 0 && 
          <ListGroup>
            {featuredList.map((item, index) => {
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
}


export default FeaturedWidget;

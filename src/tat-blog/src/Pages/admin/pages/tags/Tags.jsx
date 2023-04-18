import React, {useEffect, useState} from "react";
import { Table } from "react-bootstrap";
import { useSelector } from "react-redux";
import { Link, useParams } from "react-router-dom";
import TagFilterPane from "../../../../Components/admin/TagFilterPane";
import Loading from "../../../../Components/Loading";
import { getTagFilter } from "../../../../Services/TagCloudWidget";

const Tags = () => {
  const [tagList, setTagList] = useState([]);
  const [isVisibleLoading, setIsVisibleLoading] = useState(true),
  tagFilter = useSelector(state => state.tagFilter);

  let{id} = useParams,
  p = 1, ps = 10;

  useEffect(() =>{
    document.title ="Quản lý tags";

    getTagFilter(tagFilter, ps, p).then(data => {
      if(data){
        console.log("data: ", data);
        setTagList(data.items);
      }else{
        setTagList([]);
      }
      setIsVisibleLoading(false);

    });
  }, [tagFilter, p, ps]);

  
  return (
    <>
      <h1>
        Đây là khu vực quản lý các Tags {id}
      </h1>
      <TagFilterPane/>
      {isVisibleLoading ? (
        <Loading/>
      ) : (
        <Table striped responsive bordered>
          <thead>
            <tr>
              <th>Tên thẻ</th>
              <th>Mô tả</th>
              <th>Số bài viết</th>
              <th>Xoá</th>
            </tr>
          </thead>
          <tbody>
            {tagList.length > 0 ? (
              tagList.map((item, index) => (
                <tr key={index}>
                  <td>
                    <Link to={`/admin/tags/edit/${item.id}`}>
                      {item.name}
                    </Link>
                  </td>
                  <td>{item.description}</td>
                  <td>{item.postCount}</td>
                  <td>Xoá đi</td>
                </tr>
              ))
            ) : (
              <tr>
                <td colSpan={5}>
                  <h4 className="text-danger text-center">
                    Không tìm thấy thẻ nào
                  </h4>
                </td>
              </tr>
            )}
          </tbody>
        </Table>
      )}
    </>
  );
}

export default Tags;

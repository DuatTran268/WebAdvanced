import { faTrash } from "@fortawesome/free-solid-svg-icons";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import React, { useEffect, useState } from "react";
import { Table } from "react-bootstrap";
import { useSelector } from "react-redux";
import { Link, useParams } from "react-router-dom";
import TagFilterPane from "../../../../Components/admin/TagFilterPane";
import Loading from "../../../../Components/Loading";
import { deleteTag, getTagFilter } from "../../../../Services/TagCloudWidget";

const Tags = () => {
  const [tagList, setTagList] = useState([]);
  const [isVisibleLoading, setIsVisibleLoading] = useState(true),
    tagFilter = useSelector((state) => state.tagFilter);

  let { id } = useParams,
    p = 1,
    ps = 10;

  useEffect(() => {
    document.title = "Quản lý tags";

    getTagFilter(tagFilter, ps, p).then((data) => {
      if (data) {
        console.log("data: ", data);
        setTagList(data.items);
      } else {
        setTagList([]);
      }
      setIsVisibleLoading(false);
    });
  }, [tagFilter, p, ps]);

  // delete
  const handleDeleteTag = (e, id) => {
    e.preventDefault();
    RemovePost(id);
    async function RemovePost(id) {
      if (window.confirm("Bạn có muốn xoá bài viết này")) {
        const res = await deleteTag(id);
        if (res) {
          alert("Đã xoá bài viết");
        } else {
          alert("Đã xảy ra lỗi khi xoá bài viết");
        }
      }
    }
  };

  return (
    <>
      <h1>Đây là khu vực quản lý các Tags {id}</h1>
      <TagFilterPane />
      {isVisibleLoading ? (
        <Loading />
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
                    <Link to={`/admin/tags/edit/${item.id}`}>{item.name}</Link>
                  </td>
                  <td>{item.description}</td>
                  <td>{item.postCount}</td>
                  <td>
                    <div
                      className="text-center text-danger"
                      onClick={(e) => handleDeleteTag(e, item.id)}
                    >
                      <FontAwesomeIcon icon={faTrash} />
                    </div>
                  </td>
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
};

export default Tags;

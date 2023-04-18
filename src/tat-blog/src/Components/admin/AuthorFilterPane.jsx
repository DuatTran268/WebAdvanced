import { useEffect, useState } from "react";
import { useDispatch, useSelector } from "react-redux";
import { reset, updateKeyword } from "../../Redux/Reducer";
import { getFilterAuthor } from "../../Services/TopAuthorWidget";
import { Button, Form } from "react-bootstrap";
import { Link } from "react-router-dom";



const AuthorFilterPane = () => {

  const authorFilter = useSelector(state => state.authorFilter), dispatch = useDispatch(),
  [filter, setFilter] = useState();

  const handleReset = (e) => {
    dispatch(reset());
  }

  const handleSubmit = (e) => {
    e.preventDefault();
  };

  useEffect(() => {
    getFilterAuthor().then((data) => {
      if (data){
        setFilter(data);
      }
      else{
        setFilter([]);
      }
    })
  }, [])




  return (
    <Form method="get"
    onReset={handleReset}
    onSubmit={handleSubmit}
    className="row gy-2 gx-3 align-items-center p-2"
    >
    <Form.Group className="col-auto">
      <Form.Label className="visually-hidden">
        Keyword
      </Form.Label>
      <Form.Control 
      type="text"
      placeholder="Nhập vào từ khoá ..."
      name="name"
      value={authorFilter}
      onChange = {(e) => dispatch(updateKeyword(e.target.value))}
      />
    </Form.Group>

    <Form.Group className="col-auto">
        <Button variant="danger" type="submit">
          Xoá Lọc
        </Button>
        <Button  type="submit" className="btn btn-primary ms-2">
          Bỏ lọc
        </Button>
        <Link to='/admin/authors/edit' className="btn btn-success ms-2">Thêm mới</Link>
      </Form.Group>
    </Form>
  )
}

export default AuthorFilterPane;
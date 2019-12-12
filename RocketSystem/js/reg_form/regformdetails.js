// JavaScript source code

function onload(obj) {

    if (obj == 1) {
        document.getElementById("persname").checked = true;
        document.getElementById("Postb").checked = true;
        $(".Otherbank").attr("disabled", true);
        document.getElementById("brfocom").checked = true;
        document.getElementById("saving").checked = true;
    }

};
//===========================Check Box functions================================================//
function onlyOne1(checkbox) {

    var checkboxes = document.getElementsByName('check1')

    checkboxes.forEach((item) => {
        if (item !== checkbox) { item.checked = false }
        else { item.checked = true }
    })

    var x = document.getElementById("mydiv1");
    var y = document.getElementById("mydiv2");
    if (document.getElementById("persname").checked) {
        document.getElementById("namereq").innerHTML = "";
        if (x.style.display === "none") {
            x.style.display = "block";
            y.style.display = "none";
        }
    } else if (document.getElementById("comname").checked) {
        document.getElementById("comnamereq").innerHTML = "";
        // document.getElementById("brfocom").checked = true;
        if (y.style.display === "none") {
            y.style.display = "block";
            x.style.display = "none";

        }
    }

}
function onlyOne2(checkbox) {
    var checkboxes = document.getElementsByName('check2')
    checkboxes.forEach((item) => {
        if (item !== checkbox) { item.checked = false }
        else { item.checked = true }
    })

    var cx = document.getElementById("bcomdiv");
    var cy = document.getElementById("acomdiv");
    var cz = document.getElementById("midcomdiv");
    if (document.getElementById("brfocom").checked) {
        if (cx.style.display === "none") {
            cx.style.display = "block";
            cy.style.display = "none";
            cz.style.display = "none";
        }
    } else if (document.getElementById("aftcom").checked) {
        if (cy.style.display === "none") {
            cy.style.display = "block";
            cx.style.display = "none";
            cz.style.display = "none";
        }
    } else if (document.getElementById("midcom").checked) {
        if (cz.style.display === "none") {
            cz.style.display = "block";
            cx.style.display = "none";
            cy.style.display = "none";
        }
    }


}
function onlyOne3(checkbox) {
    var checkboxes = document.getElementsByName('check3')
    checkboxes.forEach((item) => {
        if (item !== checkbox) { item.checked = false }
        else { item.checked = true }
    })
    if (document.getElementById("Postb").checked) {

        $(".Otherbank").attr("disabled", true);
        $(".Otherbank").val("");
        $('.Postbank').removeAttr("disabled");
        document.getElementById("saving").checked = true;
        document.getElementById("bankreq").innerHTML = "";
        document.getElementById("bankuid").style.display = "none";

    } else if (document.getElementById("otherb").checked) {

        $('.Otherbank').removeAttr("disabled");
        $(".Postbank").attr("disabled", true);
        $(".Postbank").val("");
        document.getElementById("bank").checked = true;
        document.getElementById("bankname").value = "";
        document.getElementById("bankreq").innerHTML = "";


    }
}

/***************************ONFOCUSOUT FUNCTIONS************************** */

function kanjiandhiragana(inputText) {
    /*******************with spase hiragana & kanji *********** */
    //var kanaformat = /^[ 一-龥 ぁ-ん]+$/;
    var kanaformat = /^[一-龥　ぁ-ん ]+$/;
    /*******************without spase hiragana & kanji *********** */
    //var kanaformat = /^[一-龥ぁ-ん]+$/;
    if (inputText.match(kanaformat)) {
        return true;
    }
    else {
        return false;
    }
}
function halfwidthkathakana(inputText) {
    /*******************with spase half width KATAKANA *********** */
    var kanaformat = /^[ｧ-ﾝﾞﾟ ]+$/;
    /*******************without spase half width KATAKANA *********** */
    //var kanaformat = /^[ｧ-ﾝﾞﾟ]+$/;
    if (inputText.match(kanaformat)) {
        //alert("TRUE");
        return true;
    }
    else {
        //alert("false");
        return false;
    }
}
function allcharectershalfwidth(inputText) {
    /*******************all charecters half width without kanji  *********** */
    //var kanaformat = /^[\u0020-\u203E-\uFF61-\uFF9F-\u3041-\u309f]+$/;
    /*******************charecters half width hiragana(FF61-)  *********** */
    var kanaformat = /^[一-龥　ぁ-ん+\u0020-\u203E-\uFF61-\uFF9F]+$/;
    if (inputText.match(kanaformat)) {
        return true;
    }
    else {
        return false;
    }
}
function allalphanumeric(inputText) {
    /*******************All half width alphabets *********** */
    var kanaformat = /^[a-zA-Z0-9]+$/;
    /*******************All numbers ( half-width ) *********** */
    // var numformat = /^[0-9]+$/;
    if (inputText.match(kanaformat)) {
        return true;
    }
    else {
        return false;
    }
}
function validateEmail(email) {

    var re = /^(([^<>()\[\]\\.,;:\s@"]+(\.[^<>()\[\]\\.,;:\s@"]+)*)|(".+"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/;

    if (re.test(email)) {
        return true;
    }
    return false;
}
/***************************ONFOUSOUT FUNCTIONS END********************** */

///****************IS JAPANESE CHERACTER VALIDATION*************************
function namevalidate(obj) {
    var ch = obj.value;
    if (kanjiandhiragana(ch) != true) {
        document.getElementById("namereq").innerHTML = "氏名は漢字又は、ひらがなで入力必須です。";
        document.getElementById("namereq").className = "label label-pill label-danger";
        document.getElementById("namekanji").focus();
        return false;
    } else { document.getElementById("namereq").innerHTML = ""; }
    return true;
}
function companynamevalidate(obj) {

    var ch = obj.value;
    if (document.getElementById("brfocom").checked) {
        if (kanjiandhiragana(ch) != true && ch != "") {
            document.getElementById("comnamereq").innerHTML = "会社名はひらがな又は、漢字で入力しなければなりません。";
            document.getElementById("comnamereq").className = "label label-pill label-danger";
            document.getElementById("persname1").focus();
            return false;
        } else { document.getElementById("comnamereq").innerHTML = ""; }
    }
    else if (document.getElementById("aftcom").checked) {
        if (kanjiandhiragana(ch) != true && ch != "") {
            document.getElementById("comnamereq").innerHTML = "会社名はひらがな又は、漢字で入力しなければなりません。";
            document.getElementById("comnamereq").className = "label label-pill label-danger";
            document.getElementById("persname2").focus();
            return false;
        } else { document.getElementById("comnamereq").innerHTML = ""; }
    }
    else if (document.getElementById("midcom").checked) {
        if (kanjiandhiragana(ch) != true && ch != "") {
            document.getElementById("comnamereq").innerHTML = "会社名はひらがな又は、漢字で入力しなければなりません。";
            document.getElementById("comnamereq").className = "label label-pill label-danger";
            document.getElementById("midcomname1").focus();
            return false;
        } else { document.getElementById("comnamereq").innerHTML = ""; }
    }
    return true;
}
function kananamevalidate(obj) {
    var ch = obj.value;
    //******** 2019.09.11************//
    var HalfWidthkana = convertToHalfWidth(ch);
    document.getElementById("namekatha").value = HalfWidthkana;
    var newval = document.getElementById("namekatha").value;
    //=============================//

    if (halfwidthkathakana(newval) != true) {
        document.getElementById("namekathreq").innerHTML = "名前はカタカナで入力必須です。";
        document.getElementById("namekathreq").className = "label label-pill label-danger";
        document.getElementById("namekatha").focus();
        return false;
    } else { document.getElementById("namekathreq").innerHTML = ""; }

    return true;
}

//function nicknamevalidate(obj) {
//    var ch = obj.value;
//    //******** 2019.09.11************//
//    var HalfWidthkana = convertToHalfWidth(ch);
//    document.getElementById("nickname").value = HalfWidthkana;
//    var newval = document.getElementById("nickname").value;
//    //=============================//

//    if (halfwidthkathakana(newval) != true) {
//        document.getElementById("nicknamekathreq").innerHTML = "ニックネームは半角で入力必須です。";
//        document.getElementById("nicknamekathreq").className = "label label-pill label-danger";
//        document.getElementById("nickname").focus();
//        return false;
//    } else { document.getElementById("nicknamekathreq").innerHTML = ""; }
//    return true;
//}
//****************************HOLDER NAME NNNNNN*********************************
function recipientvalidate(obj) {
    var ch = obj.value;
    //******** 2019.09.11************//
    var HalfWidthkana = convertToHalfWidth(ch);
    document.getElementById("receipant").value = HalfWidthkana;
    var newval = document.getElementById("receipant").value;
    //=============================//
    if (document.getElementById("otherb").checked) {
        if (halfwidthkathakana(newval) != true) {
            document.getElementById("recekathreq").innerHTML = "　口座名義を半角カナで入力しなければなりません。";
            document.getElementById("recekathreq").className = "label label-pill label-danger";
            document.getElementById("receipant").focus();
            return false;
        } else { document.getElementById("recekathreq").innerHTML = ""; }
    }
    return true;
}
function accrecipientvalidate(obj) {
    var ch = obj.value;
    //******** 2019.09.11************//
    var HalfWidthkana = convertToHalfWidth(ch);
    document.getElementById("accreceipant").value = HalfWidthkana;
    var newval = document.getElementById("accreceipant").value;
    //=============================//

    if (document.getElementById("Postb").checked) {
        if (halfwidthkathakana(newval) != true && ch != "") {
            document.getElementById("accrecekathreq").innerHTML = "口座名義を半角カナで入力しなければなりません。";
            document.getElementById("accrecekathreq").className = "label label-pill label-danger";
            document.getElementById("accreceipant").focus();
            return false;
        } else { document.getElementById("accrecekathreq").innerHTML = ""; }
    }
    return true;
}
//**************************NNNNN***********************************
//*************************BANK NAME********************************************
function banknamevalidate(obj) {
    var ch = obj.value;
    //******** 2019.09.11************//
    var HalfWidthkana = convertToHalfWidth(ch);
    document.getElementById("bankname").value = HalfWidthkana;
    var newval = document.getElementById("bankname").value;
    //=============================//

    if (halfwidthkathakana(newval) != true) {
        document.getElementById("bankreq").innerHTML = "金融機関名を半角カナで入力しなければなりません。";
        document.getElementById("bankreq").className = "label label-pill label-danger";
        document.getElementById("bankname").focus();
        return false;
    } else { document.getElementById("bankreq").innerHTML = ""; }
    return true;
}
//*******************************************************************
function webmailvalidate(obj) {
    var ch = obj.value;
    if (validateEmail(ch) != true && ch != "") {
        document.getElementById("webmailreq").innerHTML = "ウェブメールは半角で入力必須です。";
        document.getElementById("webmailreq").className = "label label-pill label-danger";
        document.getElementById("webemail").focus();
        return false;
    } else { document.getElementById("webmailreq").innerHTML = ""; }
    return true;
}
function mobmailvalidate(obj) {
    var ch = obj.value;
    if (validateEmail(ch) != true && ch != "") {
        document.getElementById("mobemailreq").innerHTML = "携帯用メールを正しく入力必須です。";
        document.getElementById("mobemailreq").className = "label label-pill label-danger";
        document.getElementById("mobemail").focus();
        return false;
    } else { document.getElementById("mobemailreq").innerHTML = ""; }
    return true;
}
function birthdayvalnull() {
    var year = document.getElementById("year").value;
    var month = document.getElementById("month").value;
    var day = document.getElementById("day").value;
    if (year != "1111" && month != "月" && day != "日") {
        document.getElementById("birthdayreq").innerHTML = "";
    }
}
function mobilevalnull() {
    var mob1 = document.getElementById("mob1").value;
    var mob2 = document.getElementById("mob2").value;
    var mob3 = document.getElementById("mob3").value;
    if (mob1 != "" || mob2 != "" || mob3 != "") {
        document.getElementById("phonevali").innerHTML = "";
    }
}
function tellvalnull() {
    var tel1 = document.getElementById("tel1").value;
    var tel2 = document.getElementById("tel2").value;
    var tel3 = document.getElementById("tel3").value;
    if (tel1 != "" || tel2 != "" || tel3 != "") {
        document.getElementById("tellvali").innerHTML = "";
    }
}
function faxvalnull() {
    var fax1 = document.getElementById("fax1").value;
    var fax2 = document.getElementById("fax2").value;
    var fax3 = document.getElementById("fax3").value;
    if (fax1 != "" || fax2 != "" || fax3 != "") {
        document.getElementById("faxvali").innerHTML = "";
    }
}
//******************************************************* BRANCH NAME **************
function branchvalidate(obj) {
    var ch = obj.value;
    //******** 2019.09.11************//
    var HalfWidthkana = convertToHalfWidth(ch);
    document.getElementById("branchname").value = HalfWidthkana;
    var newval = document.getElementById("branchname").value;
    //=============================//
    if (halfwidthkathakana(newval) != true) {
        document.getElementById("branchreq").innerHTML = "支店名を半角カナで入力しなければなりません。";
        document.getElementById("branchreq").className = "label label-pill label-danger";
        document.getElementById("branchname").focus();
        return false;
    } else { document.getElementById("branchreq").innerHTML = ""; }
    return true;
}
function usernamevalidate(obj) {
    var ch = obj.value;
    if (allalphanumeric(ch) != true) {
        document.getElementById("usernamereq").innerHTML = "Only Alphanumeric";
        document.getElementById("usernamereq").className = "label label-pill label-danger";
        document.getElementById("username").focus();
        return false;
    } else {
        findUser(obj);
        document.getElementById("usernamereq").innerHTML = "";
    }
    return true;
}
function accountnoreqnull() {

    if (document.getElementById("accountno").value != "") {
        document.getElementById("accountnoreq").innerHTML = "";
    }
}
function accountno22reqnull(obj) {
    var ch = obj.value;
    if (ch != "") {
        document.getElementById("accountno22req").innerHTML = "";
    }
}

function bravalnull() {
    var bra1 = document.getElementById("bra1").value;
    var bra2 = document.getElementById("bra2").value;
    var bra3 = document.getElementById("bra3").value;
    if (bra1 != "" || bra2 != "" || bra3 != "") {
        document.getElementById("branchcodereq").innerHTML = "";
    }
}

function allnulluserlogins(obj) {
    var ch = obj.value;
    if (ch != "") {
        document.getElementById("questionOnereq").innerHTML = "";
        document.getElementById("answerOnereq").innerHTML = "";
        document.getElementById("questionTworeq").innerHTML = "";
        document.getElementById("answerTworeq").innerHTML = "";

    }
}

function passwordvalidate(obj) {
    var ch = obj.value;
    if (ch.length < 8) {
        document.getElementById("passwordreq").innerHTML = "パスワードには文字が8個必要です";
        document.getElementById("passwordreq").className = "label label-pill label-danger";
        document.getElementById("password").focus();
    } else {
        document.getElementById("passwordreq").innerHTML = "";
    }
}
function conpasswordvalidate(obj) {
    var ch = obj.value;
    var pswd = document.getElementById("password").value;
    if (pswd != ch || ch.length < 8) {
        document.getElementById("conpasswordreq").innerHTML = "パスワードが一致しないのでタイプしてください。";
        document.getElementById("conpasswordreq").className = "label label-pill label-danger";
        document.getElementById("confirmpassword").focus();
        return false;
    } else {
        document.getElementById("conpasswordreq").innerHTML = "";
    }
}

//////************** USER NAME ALLRADY EXISTS**************/////////
function findUser(treeid) {
    var val1 = treeid.value;
    $.ajax({
        type: "POST",
        url: "/users/findUser",
        data: JSON.stringify({ 'username': val1 }),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: successFunc,
        error: errorFunc
    });
    function successFunc(data, status) {
        document.getElementById("usernamereq").innerHTML = "Username already exists";
        document.getElementById("usernamereq").className = "label label-pill label-danger";
        document.getElementById("username").focus();

    } function errorFunc() {
        document.getElementById("usernamereq").innerHTML = "";
    }
}

//********************* IS JAPANESE CHARACTERS VALIDATION  END ********************************************

//********** SUBMIT BUTTON CLICK *********************************** 
function GrpValidate(reg) {

    if (Validateform(reg) == true) {
        concatAllSeperateTextFields();
        document.getElementById("submitform").submit();
    }
}
function Validateform(reg) {
    //field must be filled//
    /********* */
    var year = document.getElementById("year").value;
    var month = document.getElementById("month").value;
    var day = document.getElementById("day").value;
    /********* */
    var mob1 = document.getElementById("mob1").value;
    var mob2 = document.getElementById("mob2").value;
    var mob3 = document.getElementById("mob3").value;
    /********* */
    //var tel1 = document.getElementById("tel1").value;
    //var tel2 = document.getElementById("tel2").value;
    //var tel3 = document.getElementById("tel3").value;
    /********* */
    var bra1 = document.getElementById("bra1").value;
    var bra2 = document.getElementById("bra2").value;
    var bra3 = document.getElementById("bra3").value;

    var accnulval = document.getElementById("accountno").value;
    var banknulval = document.getElementById("bankname").value;
    var receipant = document.getElementById("receipant").value;
    var accreceipant = document.getElementById("accreceipant").value;
    var branchnameval = document.getElementById("branchname").value;

    var sym1 = document.getElementById("sym1").value;
    var sym2 = document.getElementById("sym2").value;
    var sym3 = document.getElementById("sym3").value;
    var sym4 = document.getElementById("sym4").value;
    var sym5 = document.getElementById("sym5").value;

    var no1 = document.getElementById("no1").value
    var no2 = document.getElementById("no2").value
    var no3 = document.getElementById("no3").value
    var no4 = document.getElementById("no4").value
    var no5 = document.getElementById("no5").value
    var no6 = document.getElementById("no6").value
    var no7 = document.getElementById("no7").value
    var no8 = document.getElementById("no8").value

    var beforcom1 = document.getElementById("persname1").value;
    var aftercom1 = document.getElementById("persname2").value;

    var poscode1 = document.getElementById("pos1").value;
    var poscode2 = document.getElementById("pos2").value;
    var kananame = document.getElementById("namekatha").value;



    if (document.getElementById("persname").checked) {
        if (document.getElementById("namekanji").value == "") {
            document.getElementById("namereq").innerHTML = "このフィールドは入力必須です。";
            document.getElementById("namereq").className = "label label-pill label-danger";
            document.getElementById("namekanji").focus();
            return false;
        }

    }
    if (document.getElementById("comname").checked) {

        if (document.getElementById("brfocom").checked) {
            if (beforcom1 == "" || document.getElementById("companytype1").value == "0") {
                document.getElementById("comnamereq").innerHTML = "会社名はひらがな又は、漢字で入力しなければなりません。";
                document.getElementById("comnamereq").className = "label label-pill label-danger";
                document.getElementById("persname1").focus();
                return false;
            }
        } else if (document.getElementById("aftcom").checked) {
            if (aftercom1 == "" || document.getElementById("companytype2").value == "0") {
                document.getElementById("comnamereq").innerHTML = "会社名はひらがな又は、漢字で入力しなければなりません。";
                document.getElementById("comnamereq").className = "label label-pill label-danger";
                document.getElementById("persname2").focus();
                return false;
            }
        } else if (document.getElementById("midcom").checked) {
            if (midcomname1 == "" || document.getElementById("companytype3").value == "0" || midcomname2 == "") {
                document.getElementById("comnamereq").innerHTML = "会社名はひらがな又は、漢字で入力しなければなりません。";
                document.getElementById("comnamereq").className = "label label-pill label-danger";
                document.getElementById("midcomname1").focus();
                return false;
            }
        }
    }
    if (kananame == "" || halfwidthkathakana(kananame) != true) {
        document.getElementById("namekathreq").innerHTML = "このフィールドは入力必須です。";
        document.getElementById("namekathreq").className = "label label-pill label-danger";
        document.getElementById("namekatha").focus();
        return false;
    }
    if (year == "1111" || month == "月" || day == "日") {
        document.getElementById("birthdayreq").innerHTML = "このフィールドは入力必須です。";
        document.getElementById("birthdayreq").className = "label label-pill label-danger";
        document.getElementById("year").focus();
        return false;
    }
    if (poscode1 == "" || poscode2 == "" || poscode1.length != 3 || poscode2.length != 4) {

        document.getElementById("pro1").innerHTML = "郵便番号を入力してください。";
        document.getElementById("pro1").className = "label label-pill label-danger";
        document.getElementById("pos1").focus();
        return false;
    }
    if (mob1 == "" || mob2 == "" || mob3 == "") {
        document.getElementById("phonevali").innerHTML = "このフィールドは入力必須です。";
        document.getElementById("phonevali").className = "label label-pill label-danger";
        document.getElementById("mob1").focus();
        return false;
    }

    //***************** E-MAIL NULL *****************************************************
    if (document.getElementById("webemail").value == "") {
        document.getElementById("webmailreq").innerHTML = "このフィールドは入力必須です。";
        document.getElementById("webmailreq").className = "label label-pill label-danger";
        document.getElementById("webemail").focus();
        return false;
    }

    // ******************check POST BANK validation*****
    if (document.getElementById("Postb").checked) {
        if (!document.getElementById('saving').checked && !document.getElementById('current').checked) {
            document.getElementById("cursave").innerHTML = "一つ選んでください。";
            document.getElementById("cursave").className = "label label-pill label-danger";
            return false;
        }
        if (accreceipant == "") {
            document.getElementById("accrecekathreq").innerHTML = "このフィールドは入力必須です。";
            document.getElementById("accrecekathreq").className = "label label-pill label-danger";
            document.getElementById("accreceipant").focus();
            return false;
        }
        if (sym1 == "" || sym2 == "" || sym3 == "" || sym4 == "" || sym5 == "" || no1 == "" || no2 == "" || no3 == "" || no4 == "" || no5 == "" || no6 == "" || no7 == "" || no8 == "") {

            document.getElementById("accountno22req").innerHTML = "このフィールドは入力必須です。";
            document.getElementById("accountno22req").className = "label label-pill label-danger";
            document.getElementById("sym1").focus();
            return false;
        }
        //if (banknulval == "") {
        //    document.getElementById("bankreq").innerHTML = "金融機関名を半角カナで入力しなければなりません。";
        //    document.getElementById("bankreq").className = "label label-pill label-danger";
        //    document.getElementById("bankname").focus();
        //    return false;
        //}
    }

    // ******************check OTHER BANK validation*****
    if (document.getElementById("otherb").checked) {

        if (banknulval == "" || banknulval == "ﾕｳﾁﾖ") {
            document.getElementById("bankreq").innerHTML = "金融機関名を半角カナで入力しなければなりません。";
            document.getElementById("bankreq").className = "label label-pill label-danger";
            document.getElementById("bankname").focus();
            return false;
        }
        if (receipant == "") {
            document.getElementById("recekathreq").innerHTML = "このフィールドは入力必須です。";
            document.getElementById("recekathreq").className = "label label-pill label-danger";
            document.getElementById("receipant").focus();
            return false;
        }
        if (document.getElementById("bank").checked && accnulval == "") {
            document.getElementById("accountnoreq").innerHTML = "このフィールドは入力必須です。";
            document.getElementById("accountnoreq").className = "label label-pill label-danger";
            document.getElementById("accountno").focus();
            return false;
        }
        if (document.getElementById("otheraccountno").value == "" && document.getElementById("bank").checked == false) {
            document.getElementById("Otherbankreq").innerHTML = "このフィールドは入力必須です。";
            document.getElementById("Otherbankreq").className = "label label-pill label-danger";
            document.getElementById("otheraccountno").focus();
            return false;
        }
        if (bra1 == "" || bra2 == "" || bra3 == "") {
            document.getElementById("branchcodereq").innerHTML = "このフィールドは入力必須です。";
            document.getElementById("branchcodereq").className = "label label-pill label-danger";
            document.getElementById("bra1").focus();
            return false;
        }
        if (branchnameval == "" || halfwidthkathakana(branchnameval) != true) {
            document.getElementById("branchreq").innerHTML = "このフィールドは入力必須です。";
            document.getElementById("branchreq").className = "label label-pill label-danger";
            document.getElementById("branchname").focus();
            return false;
        }

    }
    if (document.getElementById("comname").checked) {
        if (!document.getElementById("brfocom").checked && !document.getElementById("midcom").checked && !document.getElementById("aftcom").checked) {
            document.getElementById("comnamereq").innerHTML = "一つ選んでください。";
            document.getElementById("comnamereq").setAttribute("class", "label label-pill label-danger");
            return false;
        }
    }
    if (reg == 0) {
        //************** Select QuestionOne *****************************************
        if (document.getElementById("questionOne").value == 0) {
            document.getElementById("questionOnereq").innerHTML = "Select Question One";
            document.getElementById("questionOnereq").className = "label label-pill label-danger";
            document.getElementById("questionOne").focus();
            return false;
        }
        //*****************
        if (document.getElementById("answerOne").value == "") {
            document.getElementById("answerOnereq").innerHTML = "Type Answer One";
            document.getElementById("answerOnereq").className = "label label-pill label-danger";
            document.getElementById("answerOne").focus();
            return false;
        }

        //************** Select QuestionTwo  *****************************************
        if (document.getElementById("questionTwo").value == 0) {
            document.getElementById("questionTworeq").innerHTML = "Select Question Two";
            document.getElementById("questionTworeq").className = "label label-pill label-danger";
            document.getElementById("questionTwo").focus();
            return false;
        }
        //****************
        if (document.getElementById("answerTwo").value == "") {
            document.getElementById("answerTworeq").innerHTML = "Type Answer Two";
            document.getElementById("answerTworeq").className = "label label-pill label-danger";
            document.getElementById("answerTwo").focus();
            return false;
        }
        //************** USER NAME *****************************************
        //if (document.getElementById("username").value == "") {
        //    document.getElementById("usernamereq").innerHTML = "このフィールドは入力必須です。";
        //    document.getElementById("usernamereq").className = "label label-pill label-danger";
        //    document.getElementById("username").focus();
        //    return false;
        //}
        //**************  PASSWORD***********************************************
        if (document.getElementById("password").value == "") {
            document.getElementById("passwordreq").innerHTML = "このフィールドは入力必須です。";
            document.getElementById("passwordreq").className = "label label-pill label-danger";
            document.getElementById("password").focus();
            return false;
        }
        //************** CONFIRM PASSWORD ***************************************
        var compass = document.getElementById("confirmpassword");
        if (compass == "" || conpasswordvalidate(compass) == false) {
            document.getElementById("conpasswordreq").innerHTML = "このフィールドは入力必須です。nnnn";
            document.getElementById("conpasswordreq").className = "label label-pill label-danger";
            document.getElementById("confirmpassword").focus();
            return false;
        }
        //************************************************************************
    }

    return true;
}

function concatAllSeperateTextFields() {

    //concat   name and bank name

    if (document.getElementById("persname").checked) {
        var name = document.getElementById("namekanji").value;
        document.getElementById("nameorbanknane").value = name;
        document.getElementById("campanytype").value = 0;
    }
    if (document.getElementById("comname").checked) {
        if (document.getElementById("brfocom").checked) {

            var bname = document.getElementById("persname1").value
            var bnametype = document.getElementById("companytype1").value
            document.getElementById("nameorbanknane").value = bname + bnametype;
            document.getElementById("campanytype").value = 1;

        } else if (document.getElementById("aftcom").checked) {

            var bname = document.getElementById("persname2").value
            var bnametype = document.getElementById("companytype2").value
            document.getElementById("nameorbanknane").value = bnametype + bname;
            document.getElementById("campanytype").value = 2;

        } else if (document.getElementById("midcom").checked) {

            var bname = document.getElementById("midcomname1").value
            var bname2 = document.getElementById("midcomname2").value
            var bnametype = document.getElementById("companytype3").value
            document.getElementById("nameorbanknane").value = bname + bnametype + bname2;
            document.getElementById("campanytype").value = 3;

        }
    }

    //Concat NICKNAME
    var nickname = document.getElementById("nickname").value;
    if (nickname == "") {
        nickname = "NoValue";
    }
    document.getElementById("nickname").value = nickname;


    //Concat Birthday
    var year = document.getElementById("year").value;
    var month = document.getElementById("month").value;
    var day = document.getElementById("day").value;
    var birthday;
    if (year == "年" || month == "月" || day == "日") {
        birthday = "1111/11/11";
    }
    else {
        birthday = month + "/" + day + "/" + year;
    }
    document.getElementById("birthday").value = birthday;

    //Concat Mobile Number
    var mob1 = document.getElementById("mob1").value;
    var mob2 = document.getElementById("mob2").value;
    var mob3 = document.getElementById("mob3").value;
    var mobileNumber;
    if (mob1 == "" || mob2 == "" || mob3 == "") {
        mobileNumber = "NoValue-NoValue-NoValue";
    }
    else {
        mobileNumber = mob1 + "-" + mob2 + "-" + mob3;
    }
    document.getElementById("mobileNumber").value = mobileNumber;

    //Concat Postal Code
    var pos1 = document.getElementById("pos1").value;
    var pos2 = document.getElementById("pos2").value;
    var postalNo;
    if (pos1 == "" || pos2 == "") {
        postalNo = "NoValue-NoValue";
    }
    else {
        postalNo = pos1 + "-" + pos2;
    }
    document.getElementById("postalCode").value = postalNo;

    //Concat Address
    var add1 = document.getElementById("pref").value;
    var add2 = document.getElementById("addr").value;
    var adds3 = document.getElementById("addrnum").value;
    if (adds3 == "") {
        adds3 = "NoValue";
    }
    var address;
    if (add1 == "" || add2 == "" || adds3 == "") {
        address = "NoValue-NoValue^NoValue";
    }
    else {
        address = add1 + add2 + "^" + adds3;
    }
    document.getElementById("address1").value = address;

    //Concat Telephone Number
    var tel1 = document.getElementById("tel1").value;
    var tel2 = document.getElementById("tel2").value;
    var tel3 = document.getElementById("tel3").value;
    var telephoneNumber;
    if (tel1 == "" || tel2 == "" || tel3 == "") {
        telephoneNumber = "NoValue";
    }
    else {
        telephoneNumber = tel1 + "-" + tel2 + "-" + tel3;
    }
    document.getElementById("telephoneNumber").value = telephoneNumber;

    //Fax Number
    var fax1 = document.getElementById("fax1").value;
    var fax2 = document.getElementById("fax2").value;
    var fax3 = document.getElementById("fax3").value;
    var faxNumber;
    if (fax1 == "" || fax2 == "" || fax3 == "") {
        faxNumber = "NoValue";
    }
    else {
        faxNumber = fax1 + "-" + fax2 + "-" + fax3;
    }
    document.getElementById("faxnumber").value = faxNumber;

    //Concat  BANK NAME
    var bankvalue = document.getElementById("bankname").value;
    var banknamevalue;
    if (document.getElementById("Postb").checked) {

        banknamevalue = "ﾕｳﾁﾖ";

    } else if (document.getElementById("otherb").checked) {

        banknamevalue = bankvalue;
    }
    document.getElementById("banknameconcat").value = banknamevalue;


    //Concat Transfer Destination Bank Code
    var transferdestinationbankcode1 = document.getElementById("transferbankcode1").value;
    var transferdestinationbankcode2 = document.getElementById("transferbankcode2").value;
    var transferdestinationbankcode3 = document.getElementById("transferbankcode3").value;
    var transferdestinationbankcode4 = document.getElementById("transferbankcode4").value;
    var transferdestinationbankcode;
    if (transferdestinationbankcode1 == "" || transferdestinationbankcode2 == "" || transferdestinationbankcode3 == "" || transferdestinationbankcode4 == "") {
        transferdestinationbankcode = "Noval";
    }
    else {
        transferdestinationbankcode = transferdestinationbankcode1 + transferdestinationbankcode2 + transferdestinationbankcode3 + transferdestinationbankcode4;
    }
    document.getElementById("transferdestinationbankcode").value = transferdestinationbankcode;
    //alert(document.getElementById("transferdestinationbankcode").value + "Transfer Destination Bank Code");

    //Concat Branch Name In Katakana
    var bra1 = document.getElementById("bra1").value;
    var bra2 = document.getElementById("bra2").value;
    var bra3 = document.getElementById("bra3").value;
    var transferBranchCode;
    if (bra1 == "" || bra2 == "" || bra3 == "") {
        transferBranchCode = "NoValue";
    }
    else {
        transferBranchCode = bra1 + bra2 + bra3;
    }
    document.getElementById("transferBranchCode").value = transferBranchCode;
    //alert(document.getElementById("transferBranchCode").value + "Transfer Branch Code");


    //Get Radio button value 1
    var accountc1 = "";
    var accountc2 = "";
    if (document.getElementById("saving").checked) {

        accountc1 = 1;
    }
    else if (document.getElementById("current").checked) {
        accountc1 = 2;
    }


    //Get Radio button value 2
    if (document.getElementById("bank").checked) {
        accountc2 = 1;
    }
    else if (document.getElementById("agriculture").checked) {
        accountc2 = 2;
    }
    else if (document.getElementById("credit_association").checked) {
        accountc2 = 3;
    }
    else if (document.getElementById("labourunion").checked) {
        accountc2 = 4;
    }
    else if (document.getElementById("financial").checked) {
        accountc2 = 5;
    }
    else if (document.getElementById("fishrmen").checked) {
        accountc2 = 6;
    }
    //var accountc = accountc1 + "-" + accountc2;

    //postbank
    document.getElementById("accountClassification").value = accountc1;
    //otherbank
    document.getElementById("accountClassification2").value = accountc2;


    //Concat and check receipantname
    var recipintname = document.getElementById("receipant").value;
    var accrecipintnum = document.getElementById("accreceipant").value;
    var accreceipantname;
    if (document.getElementById("Postb").checked) {
        if (accrecipintnum == "") {
            accreceipantname = "NoValue";
        }
        else {
            accreceipantname = accrecipintnum;
        }
    }

    if (document.getElementById("otherb").checked) {

        if (recipintname == "") {
            accreceipantname = "NoValue";
        }
        else {
            accreceipantname = recipintname;
        }
    }
    document.getElementById("accreceipantnumber").value = accreceipantname;

    //Concat Transfer Account Number

    var transferAccountNumber;
    if (document.getElementById("otherb").checked) {

        if (document.getElementById("accountno").value == "" && document.getElementById("bank").checked) {
            //alert("ss1");
            transferAccountNumber = "NoValue";

        } else if (document.getElementById("bank").checked) {
            //alert("ss2");
            transferAccountNumber = document.getElementById("accountno").value;

        } else if (document.getElementById("otheraccountno").value == "") {
            //alert("ss3");
            transferAccountNumber = "NoValue";

        } else {
            //alert("ss4");
            transferAccountNumber = document.getElementById("otheraccountno").value;
        }


    }

    var sym1 = document.getElementById("sym1").value;
    var sym2 = document.getElementById("sym2").value;
    var sym3 = document.getElementById("sym3").value;
    var sym4 = document.getElementById("sym4").value;
    var sym5 = document.getElementById("sym5").value;

    var no1 = document.getElementById("no1").value
    var no2 = document.getElementById("no2").value
    var no3 = document.getElementById("no3").value
    var no4 = document.getElementById("no4").value
    var no5 = document.getElementById("no5").value
    var no6 = document.getElementById("no6").value
    var no7 = document.getElementById("no7").value
    var no8 = document.getElementById("no8").value

    if (document.getElementById("Postb").checked) {

        if (sym1 == "" || sym2 == "" || sym3 == "" || sym4 == "" || sym5 == "" || no1 == "" || no2 == "" || no3 == "" || no4 == "" || no5 == "" || no6 == "" || no7 == "" || no8 == "") {
            transferAccountNumber = "NoValue";
        }
        else {
            transferAccountNumber = sym1 + sym2 + sym3 + sym4 + sym5 + "-" + no1 + no2 + no3 + no4 + no5 + no6 + no7 + no8
        }
    }

    document.getElementById("transferAccountNumber").value = transferAccountNumber;
}
//************************ END *****************************************************************//


///************************ Only Numbers Functions ******************************************//

function isNumber(evt) {
    var iKeyCode = (evt.which) ? evt.which : evt.keyCode
    if (iKeyCode != 46 && iKeyCode > 31 && (iKeyCode < 48 || iKeyCode > 57))
        return false;
    return true;
}
function isNum(evt, obj) {
    var iKeyCode = (evt.which) ? evt.which : evt.keyCode
    //if (evt.keyCode !="080" || evt.keyCode !=  "090")
    if (iKeyCode != 48 && iKeyCode != 57 && iKeyCode != 56)
        return false;
    return true;
}

function checkInput(ob) {
    var invalidChars = /[^0-9]/gi
    if (invalidChars.test(ob.value)) {
        ob.value = ob.value.replace(invalidChars, "");
    }
};

//***************************************************** KATHAKANA CHARACTERS*********************************//

function convertToHalfWidth(string) {
    let characters = getCharacters(string);
    let halfWidthString = ''
    characters.forEach(character => {
        halfWidthString += mapToHankaku(character);
    });
    return halfWidthString;
}
function getCharacters(string) {
    return string.split("");
}
function mapToHankaku(character) {
    let zenHanMap = getZenkakuToHankakuMap();
    if (typeof zenHanMap[character] === 'undefined') {
        return character;
    } else {
        return zenHanMap[character];
    }
}
function getZenkakuToHankakuMap() {
    let zenHanMap = {
        'ァ': 'ｧ',
        'ア': 'ｱ',
        'ィ': 'ｨ',
        'イ': 'ｲ',
        'ゥ': 'ｩ',
        'ウ': 'ｳ',
        'ェ': 'ｪ',
        'エ': 'ｴ',
        'ォ': 'ｫ',
        'オ': 'ｵ',
        'カ': 'ｶ',
        'ガ': 'ｶﾞ',
        'キ': 'ｷ',
        'ギ': 'ｷﾞ',
        'ク': 'ｸ',
        'グ': 'ｸﾞ',
        'ケ': 'ｹ',
        'ゲ': 'ｹﾞ',
        'コ': 'ｺ',
        'ゴ': 'ｺﾞ',
        'サ': 'ｻ',
        'ザ': 'ｻﾞ',
        'シ': 'ｼ',
        'ジ': 'ｼﾞ',
        'ス': 'ｽ',
        'ズ': 'ｽﾞ',
        'セ': 'ｾ',
        'ゼ': 'ｾﾞ',
        'ソ': 'ｿ',
        'ゾ': 'ｿﾞ',
        'タ': 'ﾀ',
        'ダ': 'ﾀﾞ',
        'チ': 'ﾁ',
        'ヂ': 'ﾁﾞ',
        'ッ': 'ｯ',
        'ツ': 'ﾂ',
        'ヅ': 'ﾂﾞ',
        'テ': 'ﾃ',
        'デ': 'ﾃﾞ',
        'ト': 'ﾄ',
        'ド': 'ﾄﾞ',
        'ナ': 'ﾅ',
        'ニ': 'ﾆ',
        'ヌ': 'ﾇ',
        'ネ': 'ﾈ',
        'ノ': 'ﾉ',
        'ハ': 'ﾊ',
        'バ': 'ﾊﾞ',
        'パ': 'ﾊﾟ',
        'ヒ': 'ﾋ',
        'ビ': 'ﾋﾞ',
        'ピ': 'ﾋﾟ',
        'フ': 'ﾌ',
        'ブ': 'ﾌﾞ',
        'プ': 'ﾌﾟ',
        'ヘ': 'ﾍ',
        'ベ': 'ﾍﾞ',
        'ペ': 'ﾍﾟ',
        'ホ': 'ﾎ',
        'ボ': 'ﾎﾞ',
        'ポ': 'ﾎﾟ',
        'マ': 'ﾏ',
        'ミ': 'ﾐ',
        'ム': 'ﾑ',
        'メ': 'ﾒ',
        'モ': 'ﾓ',
        'ャ': 'ｬ',
        'ヤ': 'ﾔ',
        'ュ': 'ｭ',
        'ユ': 'ﾕ',
        'ョ': 'ｮ',
        'ヨ': 'ﾖ',
        'ラ': 'ﾗ',
        'リ': 'ﾘ',
        'ル': 'ﾙ',
        'レ': 'ﾚ',
        'ロ': 'ﾛ',
        'ヮ': '',
        'ワ': 'ﾜ',
        // 'ヰ': 'ﾞ  ﾟ',
        // 'ヱ': '',
        'ヲ': 'ｦ',
        'ン': 'ﾝ',
        'ヴ': 'ｳﾞ',
        // 'ヵ': '',
        // 'ヶ': '',
        // 'ヷ': '',
        // 'ヸ': '',
        // 'ヹ': '',
        // 'ヺ': '',
        '・': '･',
        'ー': 'ｰ',
        // 'ヽ': '',
        // 'ヾ': '',
        // 'ヿ': '',
    };
    return zenHanMap;
}
	//***************************************************** KATHAKANA CHARACTERS*********************************//
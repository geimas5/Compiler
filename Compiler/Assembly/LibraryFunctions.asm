PrintInt PROC
    push rbp
    mov rbp, rsp
    sub rsp, 0CCh
    MOV RDX, RCX    

    .data
        strPrintIntFormat db "%d",0

    .code
    LEA RCX, strPrintIntFormat
    call printf    

    mov rsp, rbp
    pop rbp
    ret
PrintInt ENDP

PrintDouble PROC
    push rbp
    mov rbp, rsp
    sub rsp, 0CCh
	MOV RDX, RCX    

    .data
        strPrintDoubleFormat db "%f",0

    .code
    LEA RCX, strPrintDoubleFormat
    call printf    

    mov rsp, rbp
    pop rbp
    ret
PrintDouble ENDP

PrintLine PROC
    push rbp
    mov rbp, rsp
    sub rsp, 0CCh
    MOV RDX, RCX    

    .data
        strPrintLineFormat db "%s",10,0

    .code
    LEA RCX, strPrintLineFormat
    call printf    

    mov rsp, rbp
    pop rbp
    ret
PrintLine ENDP

;PrintInt PROC
;    push rbp
;    mov rbp, rsp
;    sub rsp, 0CCh

;    mov rsp, rbp
;    pop rbp
;    ret
;PrintInt ENDP
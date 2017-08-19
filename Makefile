CSC    = csc
TARGET = exe
PRJC   = ResourceBuilder

DEBUG  = /define:_DEBUG_
SRC    = $(subst /,\,$(shell gfind src -name \*.cs))


default:
	$(CSC) $(DEBUG) /target:$(TARGET) /out:$(PRJC).exe $(SRC)


release:
	$(CSC) /target:winexe /out:bin\$(PRJC).exe $(SRC)

run:
	$(PRJC).exe
